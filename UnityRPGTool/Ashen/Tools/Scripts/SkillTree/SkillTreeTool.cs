﻿using UnityEngine;
using UnityEditor;
using Ashen.SkillTree;
using System.Collections.Generic;
using System;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;

namespace Manager
{
    public class SkillTreeTool : A_EnumeratedTool<SkillTreeTool>, I_Saveable
    {
        public SkillTree skillTree;
        [ShowInInspector]
        private Dictionary<SkillNode, int> skillNodeToLevel;
        [ShowInInspector]
        private Dictionary<SkillNode, I_ExtendedEffect> currentEffects;
        private DeliveryTool deliveryTool;

        public int skillPoints;

        public override void Initialize()
        {
            base.Initialize();
            skillNodeToLevel = new Dictionary<SkillNode, int>();
            currentEffects = new Dictionary<SkillNode, I_ExtendedEffect>();
            foreach (SkillNode node in skillTree.skillNodes)
            {
                skillNodeToLevel.Add(node, 0);
                currentEffects.Add(node, null);
            }
            
        }

        private void Start()
        {
            deliveryTool = toolManager.Get<DeliveryTool>();
        }

        public int GetSkillNodeLevel(SkillNode skillNode)
        {
            if (skillNodeToLevel.TryGetValue(skillNode, out int level))
            {
                return level;
            }
            throw new Exception("Invalid skill node");
        }

        public bool CanIncreaseSkillNode(SkillNode skillNode)
        {
            int level = GetSkillNodeLevel(skillNode);

            int maxLevel = skillNode.maxRanks;

            if (level == maxLevel)
            {
                return false;
            }

            return RequirementsMet(skillNode);
        }

        public bool RequirementsMet(SkillNode skillNode)
        {
            int level = GetSkillNodeLevel(skillNode);

            if (skillNode.hasRequirements)
            {
                foreach (I_SkillNodeRequirements requirements in skillNode.requirements)
                {
                    if (!requirements.RequirementsMet(this))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public NodeIncreaseRequestResponse RequestSkillNodeIncrease(SkillNode skillNode)
        {
            int level = GetSkillNodeLevel(skillNode);

            int maxLevel = skillNode.maxRanks;
            if (level == maxLevel)
            {
                return NodeIncreaseRequestResponse.MAX_LEVEL;
            }
            if (!RequirementsMet(skillNode))
            {
                return NodeIncreaseRequestResponse.REQUIREMENNTS_NOT_MET;
            }
            if (skillPoints < 1)
            {
                return NodeIncreaseRequestResponse.MISSING_SKILL_POINTS;
            }
            if (currentEffects.TryGetValue(skillNode, out I_ExtendedEffect currentEffect))
            {
                currentEffect?.Disable(false);
            }
            ApplySkill(skillNode, level + 1);
            return NodeIncreaseRequestResponse.SUCCESS;
        }

        private void ApplySkill(SkillNode skillNode, int rank)
        {
            if (skillNode.skill.skillType == Skill.SkillType.Ability)
            {
                Ability ability = skillNode.skill.GetAbilityForLevel(rank);
                AbilityHolder ah = toolManager.Get<AbilityHolder>();
                ah.GrantAbility(skillNode.skillName, ability);
            }
            else if (skillNode.skill.skillType == Skill.SkillType.Passive)
            {
                PassiveContainer container = skillNode.skill.GetPassiveForLevel(rank);
                SkillNodeEffectBuilder builder = container.builder;
                DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
                EffectsArgumentPack effectPack = packs.GetPack<EffectsArgumentPack>();
                foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                {
                    if (container.ScaleDeliveryPacks[(int)argument] != null)
                    {
                        effectPack.SetFloatArgument(argument, (float)container.ScaleDeliveryPacks[(int)argument]);
                    }
                }
                I_ExtendedEffect effect = builder.Build(deliveryTool, deliveryTool, packs);
                currentEffects[skillNode] = effect;
                effect.Enable();
            }
            skillNodeToLevel[skillNode]++;
            skillPoints--;
        }

        public int GetCurrentLevel(SkillNode skillNode)
        {
            if (skillNodeToLevel.TryGetValue(skillNode, out int level))
            {
                return level;
            }
            throw new Exception("Invalid skill node");
        }

        public bool IsSkillMax(SkillNode node)
        {
            if (skillNodeToLevel.TryGetValue(node, out int level))
            {
                return level == node.maxRanks;
            }
            throw new Exception("Invalid skill node");
        }

        public object CaptureState()
        {
            SkillTreeSaveData save = new SkillTreeSaveData();
            save.skills = new List<SkillSaveData>();
            foreach (KeyValuePair<SkillNode, int> skillToLevel in skillNodeToLevel)
            {
                SkillSaveData skillSaveData = new SkillSaveData();
                skillSaveData.level = skillToLevel.Value;
                skillSaveData.skillNodeName = skillToLevel.Key.skillName;
                save.skills.Add(skillSaveData);
            }
            return save;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }
            Dictionary<string, SkillNode> skillNameToSKillNode = new Dictionary<string, SkillNode>();
            foreach (KeyValuePair<SkillNode, int> nodePair in skillNodeToLevel)
            {
                skillNameToSKillNode.Add(nodePair.Key.skillName, nodePair.Key);
            }
            SkillTreeSaveData save = (SkillTreeSaveData)state;
            foreach (SkillSaveData skillSaveData in save.skills)
            {
                SkillNode skillNode = skillNameToSKillNode[skillSaveData.skillNodeName];
                skillNodeToLevel[skillNode] = skillSaveData.level;
                if (skillSaveData.level > 0)
                {
                    ApplySkill(skillNode, skillSaveData.level);
                }
            }
        }
    }

    [Serializable]
    public struct SkillTreeSaveData
    {
        public List<SkillSaveData> skills;
    }

    [Serializable]
    public struct SkillSaveData
    {
        public int level;
        public string skillNodeName;
    }

    public enum NodeIncreaseRequestResponse
    {
        SUCCESS, MAX_LEVEL, REQUIREMENNTS_NOT_MET, MISSING_SKILL_POINTS
    }
}
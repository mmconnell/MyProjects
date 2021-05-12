using Ashen.SkillTree;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour
{
    public CharacterBinderUI characterBinder;
    public Material disabledMaterial;

    [ShowInInspector]
    private SkillTreeTool skillTreeTool;
    [InlineEditor]
    public List<SkillTreeNodeUI> skillTreeUIs;

    public ScrollRect skillViewer;

    [FoldoutGroup("Valid Node"), AutoPopulate, HideLabel]
    public SkillTreeNodeColorScheme validNode;
    [FoldoutGroup("Invalid Node"), AutoPopulate, HideLabel]
    public SkillTreeNodeColorScheme invalidNode;
    public Color selectedNodeColor;
    //[FoldoutGroup("Selected Node"), AutoPopulate, HideLabel]
    //public SkillTreeNodeColorScheme selectedNode;

    public Image noviceIndicator;
    public Image veteranIndicator;
    public Image masterIndicator;

    public Color selectedIndicator;
    public Color inactiveIndicator;

    public RequirementsManager reqManager;

    private void Start()
    {
        skillTreeTool = characterBinder.boundTool.Get<SkillTreeTool>();
        UpdateNodes();
    }

    public void RegisterSkillTree(SkillTreeTool skillTreeTool)
    {
        this.skillTreeTool = skillTreeTool;
        UpdateNodes();
    }

    public void RegisterNode(SkillTreeNodeUI node)
    {
        if (skillTreeUIs == null)
        {
            skillTreeUIs = new List<SkillTreeNodeUI>();
        }
        if (!skillTreeUIs.Contains(node))
        {
            skillTreeUIs.Add(node);
        }
        UpdateNodes();
    }

    public void OnClickNode(SkillTreeNodeUI nodeUI, SkillNode node)
    {
        if (!skillTreeTool)
        {
            return;
        }
        NodeIncreaseRequestResponse response = skillTreeTool.RequestSkillNodeIncrease(node);
        if (response == NodeIncreaseRequestResponse.MAX_LEVEL || response == NodeIncreaseRequestResponse.SUCCESS)
        {
            int skilLevel = skillTreeTool.GetCurrentLevel(node);
            if (skilLevel == 1)
            {
                nodeUI.OnFirstSkillPoint();
            }
        }
        UpdateNodes();
    }

    public void UpdateScreenPosition(SkillLevel skillLevel)
    {
        noviceIndicator.color = inactiveIndicator;
        veteranIndicator.color = inactiveIndicator;
        masterIndicator.color = inactiveIndicator;
        switch (skillLevel)
        {
            case SkillLevel.Novice:
                skillViewer.horizontalNormalizedPosition = 0f;
                noviceIndicator.color = selectedIndicator;
                break;
            case SkillLevel.Veteran:
                skillViewer.horizontalNormalizedPosition = 0.5f;
                veteranIndicator.color = selectedIndicator;
                break;
            case SkillLevel.Master:
                skillViewer.horizontalNormalizedPosition = 1f;
                masterIndicator.color = selectedIndicator;
                break;
        }
    }

    public void OnClickRight(SkillTreeNodeUI skillTreeNodeUI, SkillNode skillNode)
    {
    }

    [Button]
    public void UpdateNodes()
    {
        if (skillTreeUIs == null)
        {
            return;
        }
        foreach (SkillTreeNodeUI node in skillTreeUIs)
        {
            if (!skillTreeTool)
            {
                node.OnValidOptionSkill();
                noviceIndicator.color = selectedIndicator;
                veteranIndicator.color = inactiveIndicator;
                masterIndicator.color = inactiveIndicator;
                continue;
            }
            if (skillTreeTool.RequirementsMet(node.skillNode))
            {

            }
            if (skillTreeTool.GetCurrentLevel(node.skillNode) == 0)
            {
                if (!skillTreeTool.CanIncreaseSkillNode(node.skillNode))
                {
                    node.OnDisabledSkill();
                }
                else
                {
                    if (skillTreeTool.skillPoints > 0)
                    {
                        node.OnValidOptionSkill();
                    }
                    else
                    {
                        node.OnMissingSkillPoints();
                    }
                }
            }
            else
            {
                if (skillTreeTool.CanIncreaseSkillNode(node.skillNode))
                {
                    if (skillTreeTool.skillPoints > 0)
                    {
                        node.OnValidOptionSkill();
                    }
                    else
                    {
                        node.OnMissingSkillPoints();
                    }
                }
                else if (skillTreeTool.IsSkillMax(node.skillNode))
                {
                    node.OnMaxSkill();
                }
            }
            if (node.skillNode.hasRequirements) {
                foreach (I_SkillNodeRequirements req in node.skillNode.requirements)
                {
                    if (req is OtherSkillNodeRequirement other)
                    {
                        reqManager.UpdateRequirements(other.skillNode, node.skillNode, req.RequirementsMet(skillTreeTool));
                    }
                }
            }
            node.currentValueText.text = skillTreeTool.GetCurrentLevel(node.skillNode) + "";
            EditorUtility.SetDirty(node.currentValueText);
        }
    }
}
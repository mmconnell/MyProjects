using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using static AbilityBuilder;
using System.Collections.Generic;
using Manager;

public class Skill : SerializedScriptableObject
{
    [PropertyRange(1, 10), OnValueChanged(nameof(UpdateChildren))]
    public int skillLevels;

    [EnumToggleButtons, SerializeField, HideLabel]
    public SkillType skillType;

    [Title("Base"), ShowIf(nameof(skillType), Value = SkillType.Ability), Hide, OdinSerialize, NonSerialized]
    public AbilitySkillNode abilitySkillNode;

    [Title("Base"), ShowIf(nameof(skillType), Value = SkillType.Passive), Hide, OdinSerialize, NonSerialized]
    public PassiveSkillNode passiveSkillNode;

    private void UpdateChildren()
    {
        if (abilitySkillNode != null)
        {
            abilitySkillNode.SkillLevel = skillLevels;
        }
        if (passiveSkillNode != null)
        {
            passiveSkillNode.SkillLevel = skillLevels;
        }
    }

    [Button]
    public PassiveContainer GetPassiveForLevel(int level)
    {
        PassiveContainer container = new PassiveContainer();
        if (level <= 0 || level > skillLevels)
        {
            Logger.ErrorLog("Invalid level for skill node! Skill node: " + name + " Max level of skill: " + skillLevels + " Requested Skill level: " + level);
            return container;
        }
        if (skillType != SkillType.Passive)
        {
            return container;
        }
        if (passiveSkillNode.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
        {
            container.builder = passiveSkillNode.baseAbility.statusEffect;
        }
        else
        {
            container.builder = passiveSkillNode.builder;
        }
        for (int x = 1; x <= level; x++)
        {
            PassiveSkillNodeOverride passiveOverride = passiveSkillNode.GetOverride(x);
            if (passiveOverride.options == SkillNodeOverrideOptions.Scale)
            {
                ScaleDeliveryPack scale = passiveOverride.scaleDeliveryPack;
                foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                {
                    if (argument.IsReserved())
                    {
                        continue;
                    }
                    EffectFloatArgument newArg = argument as EffectFloatArgument;
                    if (scale.scale.TryGetValue(newArg, out float value))
                    {
                        container.ScaleDeliveryPacks[(int)argument] = value;
                    }
                }
            }
            else if (passiveOverride.options == SkillNodeOverrideOptions.New)
            {
                ReplacePassiveSkill replace = passiveOverride.replaceSkillAbility;
                if (replace.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
                {
                    container = new PassiveContainer()
                    {
                        builder = replace.ability.statusEffect,
                    };
                }
                else if (replace.type == ReplaceAbilitySkillTypeInspector.Custom)
                {
                    container = new PassiveContainer()
                    {
                        builder = replace.builder,
                    };
                }
            }
        }
        return container;
    }

    [Button]
    public Ability GetAbilityForLevel(int level)
    {
        if (level <= 0 || level > skillLevels)
        {
            Logger.ErrorLog("Invalid level for skill node! Skill node: " + name + " Max level of skill: " + skillLevels + " Requested Skill level: " + level);
            return null;
        }
        if (skillType != SkillType.Ability)
        {
            return null;
        }
        AbilityBuilder builder = null;
        if (abilitySkillNode.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
        {
            builder = abilitySkillNode.baseAbility.abilityBuilder;
        }
        else
        {
            builder = abilitySkillNode.builder;
        }
        Ability ability = builder.BuildAbility();
        AbilityAction abilityAction = ability.primaryAbilityAction;
        for (int x = 1; x <= level; x++)
        {
            AbilitySkillNodeOverride skillOverride = abilitySkillNode.GetOverride(x);
            if (skillOverride.options == SkillNodeOverrideOptions.Scale)
            {
                ScaleAbility scale = skillOverride.scaleDeliveryPack;
                if (x == level)
                {
                    foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                    {
                        if (argument.IsReserved())
                        {
                            continue;
                        }
                        EffectFloatArgument newArg = argument as EffectFloatArgument;
                        if (scale.scale.TryGetValue(newArg, out float value))
                        {
                            abilityAction.SetEffectFloat(argument, value);
                        }
                    }
                }
                if (scale.enableAbilityOverride)
                {
                    HandleOverrides(abilityAction, scale.abilityOverride);
                }
            }
            else if (skillOverride.options == SkillNodeOverrideOptions.New)
            {
                ReplaceAbilitySkill replace = skillOverride.replaceSkillAbility;
                if (replace.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
                {
                    ability = replace.ability.abilityBuilder.BuildAbility();
                    abilityAction = ability.primaryAbilityAction;
                    if (replace.enableAbilityOverride)
                    {
                        HandleOverrides(abilityAction, replace.abilityOverride);
                    }
                }
                else if (replace.type == ReplaceAbilitySkillTypeInspector.Custom)
                {
                    ability = replace.builder.BuildAbility();
                }
            }

            SubAbilitySkillNodeOverrideList subAbilityList = skillOverride.subAbilitySkillNodeOverrideList;
            if (subAbilityList.option == SubAbilitySkillNodeOverrideList.SubAbilityOptions.ReplaceAll)
            {
                ability.secondaryAbilityActions = new List<SubAbilityAction>();
                foreach (SubAbilityBuilder subBuilder in subAbilityList.replacementSubAbilities)
                {
                    ability.secondaryAbilityActions.Add(subBuilder.BuildSubAbility(ability.primaryAbilityAction));
                }
            }
            else if (subAbilityList.option == SubAbilitySkillNodeOverrideList.SubAbilityOptions.Individual && ability.secondaryAbilityActions != null)
            {
                for (int subX = 0; subX < ability.secondaryAbilityActions.Count && subX < subAbilityList.subAbilities.Count; subX++)
                {
                    SubAbilitySkillNodeOverride subOverride = subAbilityList.subAbilities[subX];
                    if (subOverride == null)
                    {
                        continue;
                    }
                    if (subOverride.options == SkillNodeOverrideOptions.Scale)
                    {
                        ScaleSubAbility scaleSub = subOverride.scaleDeliveryPack;
                        SubAbilityAction subAction = ability.secondaryAbilityActions[subX];
                        foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                        {
                            if (argument.IsReserved())
                            {
                                continue;
                            }
                            EffectFloatArgument newArg = argument as EffectFloatArgument;
                            if (scaleSub.scale.TryGetValue(newArg, out float value))
                            {
                                subAction.SetEffectFloat(newArg, value);
                            }
                        }
                        HandleOverrides(subAction, scaleSub.abilityOverride);
                    }
                    else if (subOverride.options == SkillNodeOverrideOptions.New)
                    {
                        SubAbilityAction newAction = subOverride.replaceSkillAbility.BuildSubAbility(abilityAction);
                        ability.secondaryAbilityActions[subX] = newAction;
                    }
                        
                } 
            }
        }
        return ability;
    }

    private void HandleOverrides(AbilityAction ability, AbilitySkillOverride overrideAbility)
    {
        if (overrideAbility == null)
        {
            return;
        }
        if (overrideAbility.shouldOverrideTargeting)
        {
            AbilitySkillTargetingOverride targetingOverride = overrideAbility.abilityTargeting;
            if (targetingOverride.type == OverrideType.Override)
            {
                if (targetingOverride.overrideTarget != null)
                {
                    ability.customTarget = targetingOverride.overrideTarget;
                }
                if (targetingOverride.overrideRange != null)
                {
                    ability.customRange = targetingOverride.overrideRange;
                }
                if (targetingOverride.shouldOverrideAbilityTags)
                {
                    ability.CustomAbilityTags = targetingOverride.overrideAbilityTags;
                }
                ability.targetParty = targetingOverride.targetParty;
            }
            else if (targetingOverride.type == OverrideType.Replace)
            {
                AbilityTargeting targeting = targetingOverride.abilityTargeting;
                if (targeting.targetType == TargetTypeInspector.Attribute)
                {
                    if (targeting.attribute.targetAttribute != null)
                    {
                        ability.targetAttribute = targeting.attribute.targetAttribute;
                    }
                    if (targeting.attribute.overrideTarget != null)
                    {
                        ability.customTarget = targeting.attribute.overrideTarget;
                    }
                    if (targeting.attribute.overrideRange != null)
                    {
                        ability.customRange = targeting.attribute.overrideRange;
                    }
                    if (targeting.attribute.shouldOverrideAbilityTags)
                    {
                        ability.CustomAbilityTags = targeting.attribute.overrideAbilityTags;
                    }
                }
                else if (targeting.targetType == TargetTypeInspector.Custom)
                {
                    ability.customTarget = targeting.custom.target;
                    ability.customRange = targeting.custom.range;
                    ability.CustomAbilityTags = targeting.custom.abilityTags;
                }
                ability.targetParty = targeting.targetParty;
            }
        }
        if (overrideAbility.shouldOverrideDeliveryPack)
        {
            AbilityDeliveryPacks deliveryPack = overrideAbility.abilityDeliveryPacks;
            if (deliveryPack.deliveryPack != null)
            {
                ability.deliveryPack = deliveryPack.deliveryPack;
            }
        }
        if (overrideAbility.abilityRequirementsOverride != null)
        {
            AbilityRequirementsOverride abilityRequirements = overrideAbility.abilityRequirementsOverride;
            if (abilityRequirements.shouldOverrideRequirements)
            {
                if (abilityRequirements.overrideType == OverrideType.Override)
                {
                    if (ability.Requirements == null)
                    {
                        List<I_AbilityRequirement> requirements = new List<I_AbilityRequirement>();
                        foreach (AbilityRequirementsType type in abilityRequirements.overrideRequirements)
                        {
                            if (type.type == AbilityRequirementsTypeInspector.Override)
                            {
                                requirements.Add(type.requirement);
                            }
                        }
                        ability.Requirements = requirements;
                    }
                    else
                    {
                        int x = 0;
                        int y = 0;
                        List<AbilityRequirementsType> newRequirements = abilityRequirements.overrideRequirements;
                        if (newRequirements != null)
                        {
                            for (; x < ability.Requirements.Count && y < newRequirements.Count; y++)
                            {
                                AbilityRequirementsType newRequirement = newRequirements[y];
                                if (newRequirement.type == AbilityRequirementsTypeInspector.Override)
                                {
                                    ability.Requirements[x] = newRequirement.requirement;
                                    x++;
                                }
                                else if (newRequirement.type == AbilityRequirementsTypeInspector.Remove)
                                {
                                    ability.Requirements.RemoveAt(x);
                                }
                            }
                            for (; y < newRequirements.Count; x++)
                            {
                                AbilityRequirementsType newRequirement = newRequirements[y];
                                if (newRequirement.type == AbilityRequirementsTypeInspector.Override)
                                {
                                    ability.Requirements.Add(newRequirement.requirement);
                                }
                            }
                        }
                    }
                }
                else if (abilityRequirements.overrideType == OverrideType.Replace)
                {
                    ability.Requirements = abilityRequirements.requirements;
                }
            }
            if (abilityRequirements.shouldOverrideCosts)
            {
                AbilityRequirementsCost cost = abilityRequirements.costs;
                if (cost.primaryResourceValue != null)
                {
                    ability.primaryResourceCost = cost.primaryResourceValue;
                }
                if (cost.healthValue != null)
                {
                    ability.SetResourceCost(ResourceValues.Instance.health, cost.healthValue);
                }
                if (cost.customRequirements?.Count > 0)
                {
                    foreach (AbilityRequirementsCostCustom customCost in cost.customRequirements)
                    {
                        ability.SetResourceCost(customCost.resourceValue, customCost.customResourceValue);
                    }
                }
            }
            if (abilityRequirements.shouldOverrideGenerators)
            {
                AbilityRequirementsCost generate = abilityRequirements.generates;
                if (generate.primaryResourceValue != null)
                {
                    ability.primaryResourceGenerator = generate.primaryResourceValue;
                }
                if (generate.healthValue != null)
                {
                    ability.SetResourceGeneration(ResourceValues.Instance.health, generate.healthValue);
                }
                if (generate.customRequirements?.Count > 0)
                {
                    foreach (AbilityRequirementsCostCustom customGeneration in generate.customRequirements)
                    {
                        ability.SetResourceGeneration(customGeneration.resourceValue, customGeneration.customResourceValue);
                    }
                }
            }
        }
        if (overrideAbility.shouldOverrideAnimation)
        {
            AbilityAnimation abilityAnimation = overrideAbility.abilityAnimation;
            ability.animation = abilityAnimation.animation;
        }
        if (overrideAbility.shouldOverrideSpeed)
        {
            AbilitySpeed speed = overrideAbility.abilitySpeed;
            ability.speedFactor = speed.speedEquation.Value;
        }
    }

    private void HandleOverrides(SubAbilityAction ability, SubAbilitySkillOverride overrideAbility)
    {
        if (overrideAbility == null)
        {
            return;
        }
        if (overrideAbility.shouldOverrideTargeting)
        {
            SubAbilityTargeting targeting = overrideAbility.abilityTargeting;
            if (targeting.attribute.targetAttribute != null)
            {
                ability.TargetAttribute = targeting.attribute.targetAttribute;
            }
            if (targeting.attribute.overrideTarget != null)
            {
                ability.OverrideTarget = targeting.attribute.overrideTarget;
            }
            if (targeting.attribute.overrideRange != null)
            {
                ability.OverrideRange = (TargetRange)targeting.attribute.overrideRange;
            }
            if (targeting.attribute.shouldOverrideAbilityTags)
            {
                ability.OverrideAbilityTags = targeting.attribute.overrideAbilityTags;
            }
            ability.targetParty = targeting.targetParty;
            ability.relativeTarget = targeting.relativeTarget;
        }
        if (overrideAbility.shouldOverrideDeliveryPack)
        {
            AbilityDeliveryPacks deliveryPack = overrideAbility.abilityDeliveryPacks;
            if (deliveryPack.deliveryPack != null)
            {
                ability.deliveryPack = deliveryPack.deliveryPack;
            }
        }
        if (overrideAbility.shouldOverrideAnimation)
        {
            AbilityAnimation abilityAnimation = overrideAbility.abilityAnimation;
            ability.animation = abilityAnimation.animation;
        }
        if (overrideAbility.shouldOverrideSpeed)
        {
            SubAbilitySpeed speed = overrideAbility.abilitySpeed;
            ability.speedFactor = speed.speed.speedEquation.Value;
            ability.relativeSpeed = speed.relativeSpeed;
        }
    }
    
    public enum SkillType
    {
        Ability, Passive
    }
}

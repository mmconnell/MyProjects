using Manager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

public class AbilityBuilder
{
    [HideLabel, Title("Name")]
    public string name;

    [TabGroup("Targeting"), Hide]
    [OdinSerialize, NonSerialized]
    public AbilityTargeting abilityTargeting;

    [TabGroup("Delivery Pack"), Hide]
    [OdinSerialize, NonSerialized]
    public AbilityDeliveryPacks abilityDeliveryPacks;

    [TabGroup("Requirements"), Hide]
    [OdinSerialize, NonSerialized]
    public AbilityRequirements abilityRequirements;

    [TabGroup("Animation"), Hide]
    [OdinSerialize, NonSerialized]
    public AbilityAnimation abilityAnimation;

    [TabGroup("Speed"), Hide]
    [OdinSerialize, NonSerialized]
    public AbilitySpeed abilitySpeed;

    [TabGroup("Sub Abilities"), Hide]
    [OdinSerialize, NonSerialized]
    public List<SubAbilityBuilder> subAbilities;

    public Ability BuildAbility()
    {

        Ability ability = new Ability();
        ability.name = name;
        
        AbilityAction abilityAction = new AbilityAction();
        abilityAction.name = name;
        ability.primaryAbilityAction = abilityAction;

        if (abilityTargeting.targetType == TargetTypeInspector.Attribute)
        {
            abilityAction.targetAttribute = abilityTargeting.attribute.targetAttribute;
            if (abilityTargeting.attribute.overrideTarget != null)
            {
                abilityAction.customTarget = abilityTargeting.attribute.overrideTarget;
            }
            if (abilityTargeting.attribute.overrideRange != null)
            {
                abilityAction.customRange = (TargetRange)abilityTargeting.attribute.overrideRange;
            }
            if (abilityTargeting.attribute.shouldOverrideAbilityTags)
            {
                abilityAction.CustomAbilityTags = abilityTargeting.attribute.overrideAbilityTags;
            }
        }
        else if (abilityTargeting.targetType == TargetTypeInspector.Custom)
        {
            abilityAction.customTarget = abilityTargeting.custom.target;
            abilityAction.customRange = abilityTargeting.custom.range;
            abilityAction.CustomAbilityTags = abilityTargeting.custom.abilityTags;
        }

        if (abilityRequirements != null)
        {
            if (abilityRequirements.requirements != null && abilityRequirements.requirements.Count > 0)
            {
                abilityAction.Requirements = abilityRequirements.requirements;
            }
            if (abilityRequirements.hasCosts)
            {
                AbilityRequirementsCost cost = abilityRequirements.costs;
                if (cost.primaryResourceValue != null)
                {
                    abilityAction.primaryResourceCost = cost.primaryResourceValue;
                }
                if (cost.healthValue != null)
                {
                    abilityAction.SetResourceCost(ResourceValues.Instance.health, cost.healthValue);
                }
                if (cost.customRequirements?.Count > 0)
                {
                    foreach (AbilityRequirementsCostCustom customCost in cost.customRequirements)
                    {
                        abilityAction.SetResourceCost(customCost.resourceValue, customCost.customResourceValue);
                    }
                }
            }
            if (abilityRequirements.doesGenerate)
            {
                AbilityRequirementsCost generate = abilityRequirements.generates;
                if (generate.primaryResourceValue != null)
                {
                    abilityAction.primaryResourceGenerator = generate.primaryResourceValue;
                }
                if (generate.healthValue != null)
                {
                    abilityAction.SetResourceGeneration(ResourceValues.Instance.health, generate.healthValue);
                }
                if (generate.customRequirements?.Count > 0)
                {
                    foreach (AbilityRequirementsCostCustom customGeneration in generate.customRequirements)
                    {
                        abilityAction.SetResourceGeneration(customGeneration.resourceValue, customGeneration.customResourceValue);
                    }
                }
            }
        }

        if (abilitySpeed.option == AbilitySpeed.SpeedOptionInspector.SpeedFactor)
        {
            abilityAction.speedFactor = abilitySpeed.speedEquation.Value;
            abilityAction.speedCategory = AbilitySpeedCategories.Instance.defaultSpeedCategory;
        }
        else if (abilitySpeed.option == AbilitySpeed.SpeedOptionInspector.Category)
        {
            abilityAction.speedCategory = abilitySpeed.speedCategory;
            if (abilitySpeed.speedCategory.useSpeedCalculation)
            {
                abilityAction.speedFactor = null;
            }
        }
        abilityAction.animation = abilityAnimation.animation;
        abilityAction.deliveryPack = abilityDeliveryPacks.deliveryPack;
        abilityAction.targetParty = abilityTargeting.targetParty;

        if (subAbilities != null)
        {
            List<SubAbilityAction> subActions = new List<SubAbilityAction>();
            foreach (SubAbilityBuilder subActionBuilder in subAbilities)
            {
                SubAbilityAction subAction = new SubAbilityAction();
                subActions.Add(subActionBuilder.BuildSubAbility(abilityAction));
            }
            ability.secondaryAbilityActions = subActions;
        }

        ability.name = name;

        return ability;
    }

    public enum TargetParty
    {
        ALLY, ENEMY
    }

    public enum TargetTypeInspector
    {
        Attribute, Custom
    }
}

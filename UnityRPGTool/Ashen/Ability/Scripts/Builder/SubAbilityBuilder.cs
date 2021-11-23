using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using static AbilityBuilder;

[Serializable]
public class SubAbilityBuilder
{
    [TabGroup("Targeting"), Hide]
    [OdinSerialize, NonSerialized]
    public SubAbilityTargeting abilityTargeting;

    [TabGroup("Delivery Packs"), Hide]
    [OdinSerialize, NonSerialized]
    public AbilityDeliveryPacks abilityDeliveryPacks;

    [TabGroup("Animation"), Hide]
    [OdinSerialize, NonSerialized]
    public AbilityAnimation abilityAnimation;

    [TabGroup("Speed"), Hide]
    [OdinSerialize, NonSerialized]
    public SubAbilitySpeed abilitySpeed;

    public SubAbilityAction BuildSubAbility(AbilityAction parentAction)
    {
        SubAbilityAction subAbility = new SubAbilityAction();

        if (abilityTargeting.attribute.targetAttribute != null)
        {
            subAbility.TargetAttribute = abilityTargeting.attribute.targetAttribute;
        }
        if (abilityTargeting.attribute.overrideTarget != null)
        {
            subAbility.OverrideTarget = abilityTargeting.attribute.overrideTarget;
        }
        if (abilityTargeting.attribute.overrideRange != null)
        {
            subAbility.OverrideRange = (TargetRange)abilityTargeting.attribute.overrideRange;
        }
        if (abilityTargeting.attribute.shouldOverrideAbilityTags)
        {
            subAbility.OverrideAbilityTags = abilityTargeting.attribute.overrideAbilityTags;
        }
        subAbility.relativeTarget = abilityTargeting.relativeTarget;

        subAbility.animation = abilityAnimation.animation;
        subAbility.deliveryPack = abilityDeliveryPacks.deliveryPack;
        if (abilitySpeed.relativeSpeed == SubAbilityAction.RelativeSpeed.Unique)
        {
            if (abilitySpeed.speed.option == AbilitySpeed.SpeedOptionInspector.SpeedFactor)
            {
                subAbility.speedFactor = abilitySpeed.speed.speedEquation.Value;
                subAbility.speedCategory = AbilitySpeedCategories.Instance.defaultSpeedCategory;
            }
            else if (abilitySpeed.speed.option == AbilitySpeed.SpeedOptionInspector.Category)
            {
                subAbility.speedCategory = abilitySpeed.speed.speedCategory;
                if (abilitySpeed.speed.speedCategory.useSpeedCalculation)
                {
                    subAbility.speedFactor = null;
                }
            }
        }
        subAbility.relativeSpeed = abilitySpeed.relativeSpeed;
        subAbility.targetParty = abilityTargeting.targetParty;
        subAbility.parentAction = parentAction;

        return subAbility;
    }
}

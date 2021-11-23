using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

[Serializable]
public class SubAbilitySkillOverride
{
    [TabGroup("Root", "Targeting")]
    [ToggleGroup("Root/Targeting/" + nameof(shouldOverrideTargeting))]
    public bool shouldOverrideTargeting;
    [TabGroup("Root", "Targeting")]
    [ToggleGroup("Root/Targeting/" + nameof(shouldOverrideTargeting))]
    [Hide]
    public SubAbilityTargeting abilityTargeting;

    [TabGroup("Root", "Delivery Pack")]
    [ToggleGroup("Root/Delivery Pack/" + nameof(shouldOverrideDeliveryPack))]
    public bool shouldOverrideDeliveryPack;
    [TabGroup("Root", "Delivery Pack")]
    [ToggleGroup("Root/Delivery Pack/" + nameof(shouldOverrideDeliveryPack))]
    [OdinSerialize, Hide]
    public AbilityDeliveryPacks abilityDeliveryPacks;

    [TabGroup("Root", "Animation")]
    [ToggleGroup("Root/Animation/" + nameof(shouldOverrideAnimation))]
    public bool shouldOverrideAnimation;
    [TabGroup("Root", "Animation")]
    [ToggleGroup("Root/Animation/" + nameof(shouldOverrideAnimation))]
    [Hide]
    public AbilityAnimation abilityAnimation;

    [TabGroup("Root", "Speed")]
    [ToggleGroup("Root/Speed/" + nameof(shouldOverrideSpeed))]
    public bool shouldOverrideSpeed;
    [TabGroup("Root", "Speed")]
    [ToggleGroup("Root/Speed/" + nameof(shouldOverrideSpeed))]
    [Hide]
    public SubAbilitySpeed abilitySpeed;
}
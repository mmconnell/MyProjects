using System;
using Sirenix.OdinInspector;

[Serializable]
public class ScaleSubAbility : ScaleDeliveryPack
{
    [ToggleGroup(nameof(enableAbilityOverride))]
    public bool enableAbilityOverride;

    [Hide, ToggleGroup(nameof(enableAbilityOverride))]
    public SubAbilitySkillOverride abilityOverride;
}

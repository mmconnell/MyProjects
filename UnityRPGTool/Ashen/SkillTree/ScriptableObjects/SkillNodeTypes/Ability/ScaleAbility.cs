using System;
using Sirenix.OdinInspector;

[Serializable]
public class ScaleAbility : ScaleDeliveryPack
{
    [ToggleGroup(nameof(enableAbilityOverride))]
    public bool enableAbilityOverride;

    [Hide, ToggleGroup(nameof(enableAbilityOverride))]
    public AbilitySkillOverride abilityOverride;
}

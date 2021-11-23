using System;
using Sirenix.OdinInspector;

[Serializable]
public class ReplaceAbilitySkill
{
    [HideLabel, EnumToggleButtons]
    public ReplaceAbilitySkillTypeInspector type;

    [ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject)]
    public AbilitySO ability;

    [ToggleGroup(nameof(enableAbilityOverride)), ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject)]
    public bool enableAbilityOverride;

    [Hide, ToggleGroup(nameof(enableAbilityOverride)), ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject)]
    public AbilitySkillOverride abilityOverride;

    [Hide, ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.Custom)]
    public AbilityBuilder builder;

    
}

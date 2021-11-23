using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[Serializable]
public class SubAbilitySkillNodeOverrideList
{
    [EnumToggleButtons, HideLabel]
    public SubAbilityOptions option;

    [Hide, ShowIf(nameof(option), Value = SubAbilityOptions.ReplaceAll)]
    public List<SubAbilityBuilder> replacementSubAbilities;

    [Hide, ShowIf(nameof(option), Value = SubAbilityOptions.Individual)]
    public List<SubAbilitySkillNodeOverride> subAbilities;

    public enum SubAbilityOptions
    {
        Individual, ReplaceAll
    }
}

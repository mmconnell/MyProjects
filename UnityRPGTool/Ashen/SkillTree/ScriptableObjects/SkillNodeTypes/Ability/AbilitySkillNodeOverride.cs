using System;
using Sirenix.OdinInspector;

[Serializable]
public class AbilitySkillNodeOverride
{
    [EnumToggleButtons]
    public SkillNodeOverrideOptions options;

    [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.Scale)]
    public ScaleAbility scaleDeliveryPack;

    [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.New)]
    public ReplaceAbilitySkill replaceSkillAbility;

    [Hide, FoldoutGroup("Sub Abilities")]
    public SubAbilitySkillNodeOverrideList subAbilitySkillNodeOverrideList;
}

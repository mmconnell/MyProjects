using Sirenix.OdinInspector;
using System;

[Serializable]
public class PassiveSkillNodeOverride
{
    [EnumToggleButtons]
    public SkillNodeOverrideOptions options;

    [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.Scale)]
    public ScaleDeliveryPack scaleDeliveryPack;

    [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.New)]
    public ReplacePassiveSkill replaceSkillAbility;
}

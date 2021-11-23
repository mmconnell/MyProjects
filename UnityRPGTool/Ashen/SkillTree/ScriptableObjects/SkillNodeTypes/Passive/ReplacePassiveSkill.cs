using Sirenix.OdinInspector;
using Ashen.DeliverySystem;
using System;
using Ashen.SkillTree;

[Serializable]
public class ReplacePassiveSkill
{
    [HideLabel, EnumToggleButtons]
    public ReplaceAbilitySkillTypeInspector type;

    [ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject)]
    public PassiveScriptableObject ability;

    [ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.Custom)]
    [Hide]
    public SkillNodeEffectBuilder builder;
}

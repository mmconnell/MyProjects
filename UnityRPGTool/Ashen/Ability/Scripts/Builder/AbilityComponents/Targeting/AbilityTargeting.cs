using Sirenix.OdinInspector;
using System;
using static AbilityBuilder;

[Serializable]
public class AbilityTargeting
{
    [TitleGroup("Target Type"), EnumToggleButtons, HideLabel]
    public TargetTypeInspector targetType;

    [Hide, ShowIf(nameof(targetType), Value = TargetTypeInspector.Custom)]
    public AbilityTargetingCustom custom;

    [Hide, ShowIf(nameof(targetType), Value = TargetTypeInspector.Attribute)]
    public AbilityTargetingAttribute attribute;
    
    [EnumToggleButtons, Title("Who to target"), HideLabel]
    public TargetParty targetParty;
}

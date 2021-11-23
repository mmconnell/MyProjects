using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System;
using static AbilityBuilder;
using System.Collections.Generic;
using static SubAbilityAction;

[Serializable]
public class SubAbilityTargeting
{
    [EnumToggleButtons]
    public SubAbilityRelativeTarget relativeTarget;

    [Hide]
    public AbilityTargetingAttribute attribute;
    public List<AbilityTag> addAbilityTags;

    [EnumToggleButtons, Title("Who to target"), HideLabel]
    public TargetParty targetParty;
}

using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;
using static SubAbilityAction;

[Serializable]
public class SubAbilitySpeed
{
    [EnumToggleButtons]
    public RelativeSpeed relativeSpeed;
    [ShowIf(nameof(relativeSpeed), Value = RelativeSpeed.Unique)]
    public AbilitySpeed speed;
}

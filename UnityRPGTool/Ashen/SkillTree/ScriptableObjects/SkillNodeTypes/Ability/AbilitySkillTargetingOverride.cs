using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using static AbilityBuilder;

[Serializable]
public class AbilitySkillTargetingOverride
{
    [HideLabel, EnumToggleButtons]
    public OverrideType type;

    [ShowIf(nameof(type), OverrideType.Override)]
    public TargetRange? overrideRange;
    [ShowIf(nameof(type), OverrideType.Override)]
    public Target overrideTarget;
    [ShowIf(nameof(type), OverrideType.Override)]
    [ToggleGroup(nameof(shouldOverrideAbilityTags))]
    public bool shouldOverrideAbilityTags;
    [ShowIf(nameof(type), OverrideType.Override)]
    [ToggleGroup(nameof(shouldOverrideAbilityTags))]
    public List<AbilityTag> overrideAbilityTags;
    [EnumToggleButtons, Title("Who to target"), HideLabel]
    [ShowIf(nameof(type), OverrideType.Override)]
    public TargetParty targetParty;

    [Hide]
    [ShowIf(nameof(type), OverrideType.Replace)]
    public AbilityTargeting abilityTargeting;
}

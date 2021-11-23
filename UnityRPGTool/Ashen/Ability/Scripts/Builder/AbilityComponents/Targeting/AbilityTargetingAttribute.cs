using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using static AbilitySO;
using System.Collections.Generic;
using System;
using Sirenix.Serialization;

[Serializable]
public class AbilityTargetingAttribute
{
    public TargetAttribute targetAttribute;
    [FoldoutGroup("Override Targeting")]
    public TargetRange? overrideRange;
    [FoldoutGroup("Override Targeting")]
    public Target overrideTarget;
    [ToggleGroup("Override Targeting/" + nameof(shouldOverrideAbilityTags))]
    public bool shouldOverrideAbilityTags;
    [ToggleGroup("Override Targeting/" + nameof(shouldOverrideAbilityTags))]
    [ShowIf(nameof(shouldOverrideAbilityTags))]
    public List<AbilityTag> overrideAbilityTags;

}

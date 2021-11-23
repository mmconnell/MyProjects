using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using static AbilitySO;
using System.Collections.Generic;
using System;

[Serializable]
public class AbilityTargetingCustom
{
    public TargetRange range;
    public Target target;
    public List<AbilityTag> abilityTags;
}

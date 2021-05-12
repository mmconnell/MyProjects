using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShieldSortStruct
{
    [HorizontalGroup(""), HideLabel]
    public ShieldSortAttributes attribute;
    [HorizontalGroup(""), EnumToggleButtons, HideLabel]
    public ShieldSortOrder order;
}

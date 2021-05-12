using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A damage type defines what resistances an attack must be compared to when applied to a character
     **/
    [CreateAssetMenu(fileName = nameof(DamageType), menuName = "Custom/Enums/" + nameof(DamageTypes) + "/Type")]
    public class DamageType : A_EnumSO<DamageType, DamageTypes>
    {
    }
}
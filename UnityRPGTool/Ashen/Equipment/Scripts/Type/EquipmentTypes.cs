using UnityEngine;
using System.Collections;

namespace Ashen.EquipmentSystem
{
    [CreateAssetMenu(fileName = nameof(EquipmentTypes), menuName = "Custom/Enums/" + nameof(EquipmentTypes) + "/Types")]
    public class EquipmentTypes : A_EnumList<EquipmentType, EquipmentTypes>
    {

    }
}
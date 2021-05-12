using UnityEngine;
using System.Collections;

namespace Ashen.EquipmentSystem
{
    [CreateAssetMenu(fileName = nameof(EquipmentType), menuName = "Custom/Enums/" + nameof(EquipmentTypes) + "/Type")]
    public class EquipmentType : A_EnumSO<EquipmentType, EquipmentTypes>
    {

    }
}
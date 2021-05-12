using UnityEngine;
using System.Collections;

namespace Ashen.EquipmentSystem
{
    [CreateAssetMenu(fileName = nameof(EquipmentRarity), menuName = "Custom/Enums/" + nameof(EquipmentRarities) + "/Type")]
    public class EquipmentRarity : A_EnumSO<EquipmentRarity, EquipmentRarities>
    {

    }
}
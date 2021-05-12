using UnityEngine;
using System.Collections;

namespace Ashen.EquipmentSystem
{
    [CreateAssetMenu(fileName = nameof(EquipmentRarities), menuName = "Custom/Enums/" + nameof(EquipmentRarities) + "/Types")]
    public class EquipmentRarities : A_EnumList<EquipmentRarity, EquipmentRarities>
    {

    }
}
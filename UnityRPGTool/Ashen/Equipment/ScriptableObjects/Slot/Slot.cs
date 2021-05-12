using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquipmentSystem
{
    /**
     * A Slot is where a gem can be equipped
     **/
    [CreateAssetMenu(fileName = "Slot", menuName = "Custom/Enums/Slots/Type")]
    public class Slot : A_EnumSO<Slot, Slots>
    {
        public SlotGroup slotGroup;
    }
}
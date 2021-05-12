using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquipmentSystem
{
    /**
     * A Slot is where a gem can be equipped
     **/
    [CreateAssetMenu(fileName = "SlotGroup", menuName = "Custom/Enums/Slots/SlotGroups/Type")]
    public class SlotGroup : A_EnumSO<SlotGroup, SlotGroups>
    {
        public List<Slot> slots;
    }
}
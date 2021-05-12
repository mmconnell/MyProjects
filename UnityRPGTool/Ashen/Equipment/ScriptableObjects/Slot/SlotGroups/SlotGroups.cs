using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquipmentSystem
{
    /**
     * See Slot
     **/
    [CreateAssetMenu(fileName = "SlotGroups", menuName = "Custom/Enums/Slots/SlotGroups/Types")]
    public class SlotGroups : A_EnumList<SlotGroup, SlotGroups>
    { }
}
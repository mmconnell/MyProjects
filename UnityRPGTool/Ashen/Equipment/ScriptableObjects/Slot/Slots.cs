using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquipmentSystem
{
    /**
     * See Slot
     **/
    [CreateAssetMenu(fileName = nameof(Slots), menuName = "Custom/Enums/" + nameof(Slots) + "/Types")]
    public class Slots : A_EnumList<Slot, Slots>
    { }
}
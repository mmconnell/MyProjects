using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(ShiftCategories), menuName = "Custom/Enums/" + nameof(ShiftCategories) + "/Types")]
    public class ShiftCategories : A_EnumList<ShiftCategory, ShiftCategories>
    {

    }
}
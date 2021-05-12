using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * See StatusTrigger
     **/
    [CreateAssetMenu(fileName = nameof(ExtendedEffectTriggers), menuName = "Custom/Enums/" + nameof(ExtendedEffectTriggers) + "/Types")]
    public class ExtendedEffectTriggers : A_EnumList<ExtendedEffectTrigger, ExtendedEffectTriggers>
    {
        public ExtendedEffectTrigger Tick;

        public ExtendedEffectTrigger UpdateTick;
        public ExtendedEffectTrigger TurnStart;
        public ExtendedEffectTrigger TurnEnd;
    }
}
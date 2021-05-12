using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class EffectOverTimeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize, HideLabel, Indent]
        private I_EffectBuilder effect = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            I_Effect effect = this.effect.Build(owner, target, deliveryArguments);
            if (effect == null)
            {
                return null;
            }
            return new EffectOverTimeComponent(effect);
        }
    }
}
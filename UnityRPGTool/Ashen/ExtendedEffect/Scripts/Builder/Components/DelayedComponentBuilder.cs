using UnityEngine;
using System.Collections;
using Sirenix.Serialization;

namespace Ashen.DeliverySystem
{
    public class DelayedComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        public I_EffectBuilder effect;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            I_Effect effect = this.effect.Build(owner, target, deliveryArgument);
            if (effect == null)
            {
                return null;
            }
            return new DelayedComponent
            {
                effect = effect
            };
        }
    }
}
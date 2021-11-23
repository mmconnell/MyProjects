using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class TriggeredComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private ExtendedEffectTrigger[] triggers = default;
        [OdinSerialize, HideLabel, Indent]
        private I_EffectBuilder effect = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new TriggeredComponent(effect.Build(owner, target, deliveryArgument), triggers);
        }
    }
}
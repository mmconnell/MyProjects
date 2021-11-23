using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Ashen.DeliverySystem
{
    public class TargetRangeAttributeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private TargetAttribute attribute = default;
        [OdinSerialize]
        private int priority = default;
        [BoxGroup("Equation"), OdinSerialize, HideLabel]
        private TargetRange range = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new TargetRangeAttributeComponent(attribute, range, priority);
        }
    }
}
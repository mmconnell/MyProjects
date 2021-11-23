using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Ashen.DeliverySystem
{
    public class TargetAttributeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private TargetAttribute attribute = default;
        [OdinSerialize]
        private int priority = default;
        [BoxGroup("Equation"), OdinSerialize, HideLabel]
        private Target target = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new TargetAttributeComponent(attribute, this.target, priority);
        }
    }
}
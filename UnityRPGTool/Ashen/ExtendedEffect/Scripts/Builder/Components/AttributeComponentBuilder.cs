using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;
using Ashen.VariableSystem;

namespace Ashen.DeliverySystem
{
    public class AttributeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private DerivedAttribute attributeType = default;
        [OdinSerialize]
        private ShiftCategory shiftCategory = default;
        [OdinSerialize, Hide]
        private ScalingValueBuilder value;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new AttributeComponent(attributeType, shiftCategory, value.Build(owner, target, deliveryArgument));
        }
    }
}
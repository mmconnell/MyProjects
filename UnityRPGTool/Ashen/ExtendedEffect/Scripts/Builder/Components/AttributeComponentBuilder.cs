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
        [BoxGroup("Equation"), OdinSerialize, HideLabel]
        private Reference<I_Equation> equation = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new AttributeComponent(attributeType, shiftCategory, equation.Value.Calculate(owner, target, deliveryArgument.GetPack<EquationArgumentPack>()));
        }
    }
}
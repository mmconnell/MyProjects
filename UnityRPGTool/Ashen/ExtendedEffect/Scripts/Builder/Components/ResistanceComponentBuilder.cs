using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Ashen.VariableSystem;
using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    public class ResistanceComponentBuilder : I_ComponentBuilder
    {

        [HorizontalGroup(nameof(ResistanceComponent))]

        [VerticalGroup(nameof(ResistanceComponent) + "/" + nameof(ResistanceType))]
        [OdinSerialize, EnumSODropdown, HideLabel, Title("Resistance Type")]
        private DamageType ResistanceType = default;
        [VerticalGroup(nameof(ResistanceComponent) + "/" + nameof(shiftCategory))]
        [OdinSerialize, HideLabel, Title("Shift Category"), EnumSODropdown]
        private ShiftCategory shiftCategory = default;
        [VerticalGroup(nameof(ResistanceComponent) + "/ResistanceAmount")]
        [OdinSerialize, HideLabel, Title("Equation")]
        private Reference<I_Equation> equation = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new ResistanceComponent
            {
                ResistanceType = ResistanceType,
                shiftCategory = shiftCategory,
                value = equation.Value.Calculate(owner, target, deliveryArgument.GetPack<EquationArgumentPack>())
            };
        }
    }
}
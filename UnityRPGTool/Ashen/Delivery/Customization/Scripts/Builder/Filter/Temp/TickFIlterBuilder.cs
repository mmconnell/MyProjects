using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Ashen.EquationSystem;
using Ashen.VariableSystem;

namespace Ashen.DeliverySystem
{
    public class TickFIlterBuilder : I_FilterBuilder
    {
        [OdinSerialize]
        private I_FilterBuilder filter = default;
        [OdinSerialize]
        private Reference<I_Equation> ticks = default;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target)
        {
            int result = (int)ticks.Value.Calculate(owner, null);
            return new TickFilter(filter.Build(owner, target), result);
        }
    }
}
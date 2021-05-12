using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Ashen.EquationSystem;
using Ashen.VariableSystem;

namespace Ashen.DeliverySystem
{
    public class DamageShieldFilterBuilder : I_FilterBuilder
    {
        [OdinSerialize]
        private DamageContainer damageContainer = default;
        [OdinSerialize]
        private Reference<I_Equation> total = default;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target)
        {
            int result = (int)total.Value.Calculate(owner, null);
            return new DamageShieldFilter(damageContainer, result);
        }
    }
}
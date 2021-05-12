using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class ReduceDamageFilterBuilder : I_FilterBuilder
    {
        [OdinSerialize, Hide, Title("Damage Types")]
        private DamageContainer damageContainer = default;
        [OdinSerialize]
        private int amountToReduceBy = default;
        [OdinSerialize]
        private bool reduceFromEach = default;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target)
        {
            return new ReduceDamageFilter(damageContainer, amountToReduceBy, reduceFromEach);
        }
    }
}
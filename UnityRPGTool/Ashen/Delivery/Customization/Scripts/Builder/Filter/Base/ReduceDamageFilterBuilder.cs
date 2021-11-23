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
        [OdinSerialize, HideIf(nameof(percentage))]
        private ScalingValueBuilder amountToReduceBy = default;
        [OdinSerialize, HideIf(nameof(percentage))]
        private bool reduceFromEach = default;
        [OdinSerialize]
        private bool percentage = default;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            return new ReduceDamageFilter(damageContainer, (amountToReduceBy == null ? 0 : (int)amountToReduceBy.Build(owner, target, arguments)), reduceFromEach, percentage);
        }
    }
}
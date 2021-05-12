using UnityEngine;
using Manager;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System;

namespace Ashen.DeliverySystem
{
    /**
     * This is a filter that will reduce the damage of any DamageType inside of the passed in DamageContainer.
     **/
    [Serializable]
    public class ReduceDamageFilter : A_BaseFilter
    {
        private DamageContainer damageContainer;
        private int amountToReduceBy;
        private bool reduceFromEach;

        public ReduceDamageFilter() { }

        public ReduceDamageFilter(DamageContainer damageContainer, int amountToReduceBy, bool reduceFromEach)
        {
            this.damageContainer = damageContainer;
            this.amountToReduceBy = amountToReduceBy;
            this.reduceFromEach = reduceFromEach;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            return FilterUtility.ReduceDamage(damageContainer, deliveryResult, amountToReduceBy, -1, reduceFromEach) > 0;
        }
    }
}
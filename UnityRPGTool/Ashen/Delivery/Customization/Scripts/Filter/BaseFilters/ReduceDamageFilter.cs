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
        private bool percentage;

        public ReduceDamageFilter() { }

        public ReduceDamageFilter(DamageContainer damageContainer, int amountToReduceBy, bool reduceFromEach, bool percentage)
        {
            this.damageContainer = damageContainer;
            this.amountToReduceBy = amountToReduceBy;
            this.reduceFromEach = reduceFromEach;
            this.percentage = percentage;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            if (!percentage)
            {
                return FilterUtility.ReduceDamage(damageContainer, deliveryResult, amountToReduceBy, -1, reduceFromEach) > 0;
            }
            bool reduced = false;
            DamageResult dr = deliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            foreach (DamageType damageType in DamageTypes.Instance)
            {
                int value = dr.GetDamage(damageType);
                if (value <= 0)
                {
                    continue;
                }
                int reduceBy = (int)(value / 2f);
                if (reduceBy == 0)
                {
                    continue;
                }
                dr.AddDamage(damageType, -reduceBy);
                reduced = true;
            }
            return reduced;
        }
    }
}
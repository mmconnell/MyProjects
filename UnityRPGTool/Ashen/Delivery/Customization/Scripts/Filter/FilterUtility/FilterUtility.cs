using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    /**
     * This class is used to consolidate logic that might be shared between two or more filters
     **/
    public class FilterUtility
    {
        public static int ReduceDamage(A_EnumContainer<DamageType, DamageTypes> damageContainer, DeliveryResultPack deliveryResult, int reduceBy, int reduceMax, bool reduceFromEach)
        {
            bool reduceAll = reduceBy == -1;
            bool noMax = reduceMax == -1;
            DamageResult damageResult = deliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            if (reduceAll && noMax)
            {
                ReduceAllDamage(damageContainer, damageResult);
            }
            int totalDamageToReduce = reduceBy;
            if (reduceAll)
            {
                totalDamageToReduce = reduceMax;
            }
            if (!noMax && !reduceAll)
            {
                totalDamageToReduce = Mathf.Min(reduceMax, reduceBy);
            }
            int totalDamageReduced = 0;
            foreach (DamageType damageTypeEnum in damageContainer.enums)
            {
                if (totalDamageToReduce < 1)
                {
                    break;
                }
                int toReduce = Mathf.Min(damageResult.GetDamage(damageTypeEnum), totalDamageToReduce);
                toReduce = Mathf.Max(0, toReduce);
                damageResult.AddDamage(damageTypeEnum, -toReduce);
                if (!reduceFromEach)
                {
                    totalDamageToReduce -= toReduce;
                }
                totalDamageReduced += toReduce;
            }
            return totalDamageReduced;
        }

        public static int ReduceAllDamage(A_EnumContainer<DamageType, DamageTypes> damageContainer, DamageResult deliveryResult)
        {
            int total = 0;
            foreach (DamageType damageType in damageContainer.enums)
            {
                if (deliveryResult.GetDamage(damageType) > 0)
                {
                    total += deliveryResult.GetDamage(damageType);
                    deliveryResult.ResetDamage(damageType);
                }
            }
            return total;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ConvertDamageFilter : A_BaseFilter
    {
        private DamageContainer fromDamageTypes = default;
        private DamageType toDamageType = default;

        public ConvertDamageFilter(DamageContainer fromDamageTypes, DamageType toDamageType)
        {
            this.fromDamageTypes = fromDamageTypes;
            this.toDamageType = toDamageType;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            DamageResult damageResult = deliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            int total = 0;
            foreach (DamageType damageType in fromDamageTypes.enums)
            {
                if (damageResult.GetDamage(damageType) > 0)
                {
                    total += damageResult.GetDamage(damageType);
                    damageResult.ResetDamage(damageType);
                }
            }
            damageResult.AddDamage(toDamageType, total);
            return total > 0;
        }
    }
}
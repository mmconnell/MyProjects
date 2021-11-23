using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using Manager;
using System.Security.Permissions;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class CollectiveDamagePack : I_Effect, ISerializable
    {
        private List<DamageType> damageTypes;
        private float value;

        public CollectiveDamagePack() { }
        public CollectiveDamagePack(List<DamageType> damageTypes, float value)
        {
            this.damageTypes = damageTypes;
            this.value = value;
        }

        public int GetAmount(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return (int)value;
        }

        public void Apply(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            int total = GetAmount(dOwner, dTarget, deliveryArguments);
            if (total == 0)
            {
                return;
            }
            DamageResult deliveryResult = targetDeliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            deliveryResult.AddDamage(deliveryResult.combineInto, total);
            foreach (DamageType dt in damageTypes)
            {
                deliveryResult.EnableDamageType(dt);
            }
        }

        protected CollectiveDamagePack(SerializationInfo info, StreamingContext context)
        {
            value = (float)info.GetValue(nameof(value), typeof(float));
            damageTypes = new List<DamageType>();
            int total = (int)info.GetValue(nameof(damageTypes) + "Count", typeof(int));
            for (int x = 0; x < total; x++)
            {
                damageTypes.Add(DamageTypes.Instance[(int)info.GetValue(nameof(damageTypes) + "[" + x + "]", typeof(int))]);
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(value), value);
            info.AddValue(nameof(damageTypes) + "Count", damageTypes.Count);
            for (int x = 0; x < damageTypes.Count; x++)
            {
                info.AddValue(nameof(damageTypes) + "[" + x + "]", (int)damageTypes[x]);
            }
        }
    }
}
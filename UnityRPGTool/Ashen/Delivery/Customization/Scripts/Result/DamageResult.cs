using UnityEngine;
using System.Collections;
using Manager;

namespace Ashen.DeliverySystem
{
    public class DamageResult : A_DeliveryResult
    {
        private int[] damageDone;
        public int[] DamageDone {
            get
            {
                return damageDone;
            }
        }
        public bool Critical { get; set; }

        public DamageResult()
        {
            damageDone = new int[DamageTypes.Count];
            Critical = false;
        }

        public override void Clear()
        {
            for (int x = 0; x < damageDone.Length; x++)
            {
                damageDone[x] = 0;
            }
            Critical = false;
        }

        public override void Calculate(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryArgumentPacks deliveryArguments)
        {
            ToolManager target = (dTarget as DeliveryTool).toolManager;
            DamageTool damageTool = target.Get<DamageTool>();
            int[] damageArray = damageDone;
            for (int x = 0; x < damageArray.Length; x++)
            {
                int damage = damageArray[x];
                if (damage != 0)
                {
                    DamageType damageType = DamageTypes.Instance[x];
                    damageArray[x] = damageTool.GetDamage(damageType, damage, deliveryArguments);
                }
            }
        }

        public override void Deliver(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryArgumentPacks deliveryArguments)
        {
            ToolManager target = (dTarget as DeliveryTool).toolManager;
            int x = 0;
            DamageTool damageTool = target.Get<DamageTool>();
            foreach (int damage in damageDone)
            {
                if (damage != 0)
                {
                    DamageType damageType = DamageTypes.Instance[x];
                    damageTool.TakeDamageRaw(damageType, damage, true);
                }
                x++;
            }
        }

        public override A_DeliveryResult Clone()
        {
            return new DamageResult();
        }
    }
}
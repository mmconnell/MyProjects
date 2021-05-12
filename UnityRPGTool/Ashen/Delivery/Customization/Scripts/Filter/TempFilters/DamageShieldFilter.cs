using UnityEngine;
using System.Collections;
using Manager;
using Sirenix.Serialization;
using Ashen.EquationSystem;
using Ashen.VariableSystem;
using System;

namespace Ashen.DeliverySystem
{
    /**
     * This TempFilter will automatically be removed after it blocks
     * a set amount of damage
     **/
    [Serializable]
    public class DamageShieldFilter : A_TempFilter
    {
        private DamageContainer damageContainer = default;
        private int shieldTotal;

        public DamageShieldFilter()
        {}

        public DamageShieldFilter(DamageContainer damageContainer, int shieldTotal)
        {
            this.damageContainer = damageContainer;
            this.shieldTotal = shieldTotal;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            int totalReduced = FilterUtility.ReduceDamage(damageContainer, deliveryResult, -1, shieldTotal, false);
            shieldTotal -= totalReduced;
            return totalReduced > 0;
        }

        public override bool Enabled()
        {
            return shieldTotal > 0 && base.Enabled();
        }
    }
}

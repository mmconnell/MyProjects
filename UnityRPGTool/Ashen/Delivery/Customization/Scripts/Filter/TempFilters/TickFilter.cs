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
     * A TickFilter will apply x amount of times before it will automatically get removed
     **/
    [Serializable]
    public class TickFilter : A_TempFilter
    {
        private I_Filter filter = default;
        private int numTicks;

        public TickFilter() { }

        public TickFilter(I_Filter filter, int numTicks)
        {
            this.filter = filter;
            this.numTicks = numTicks;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            if (filter.Apply(owner, target, deliveryArgumentsPack, deliveryResult))
            {
                numTicks--;
                return true;
            }
            return false;
        }

        public override bool Enabled()
        {
            return base.Enabled() && numTicks > 0 && filter.Enabled();
        }
    }
}

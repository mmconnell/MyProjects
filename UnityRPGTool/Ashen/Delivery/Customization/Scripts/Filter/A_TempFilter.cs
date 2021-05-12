using UnityEngine;
using System.Collections;
using Manager;
using System;

namespace Ashen.DeliverySystem
{
    /**
     * A TempFilter is one that can be disabled after a condition is met. This could be for a number of reasons.
     * Ex. A damage shield that only blocks x amount of damage before it is removed or a filter that will be removed
     * after it has successfully been activated x amount of times
     **/
    [Serializable]
    public abstract class A_TempFilter : I_Filter
    {
        private bool disabled;

        public abstract bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult);

        public void Disable()
        {
            disabled = true;
        }

        public virtual bool Enabled()
        {
            return !disabled;
        }
    }
}
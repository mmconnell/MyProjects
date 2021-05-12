using UnityEngine;
using System.Collections;
using Manager;
using System;

namespace Ashen.DeliverySystem
{
    /**
     * A conditional filter is a wrapper for a BaseFilter, that will only apply the BaseFilter if its 
     * Check method returns true. ConditionalFilters must override this class
     **/
    [Serializable]
    public abstract class A_ConditionalFilter : I_FilterBuilder
    {
        private I_FilterBuilder filter;

        public A_ConditionalFilter(I_FilterBuilder filter)
        {
            this.filter = filter;
        }

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target)
        {
            if (Check(owner, target))
            {
                return filter.Build(owner, target);
            }
            return null;
        }

        protected abstract bool Check(I_DeliveryTool owner, I_DeliveryTool target);
    }
}
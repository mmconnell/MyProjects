using UnityEngine;
using System.Collections;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class FilterComponentBuilder : I_ComponentBuilder
    {
        [Serializable]
        public enum FilterType
        {
            PRE_DEFENSIVE,
            POST_DEFENSIVE,
            PRE_OFFENSIVE,
            POST_OFFENSIVE
        }

        [OdinSerialize, EnumToggleButtons, HideLabel, Title("Type")]
        private FilterType filterType = default;

        [OdinSerialize, HideLabel, Title("Filter"), InlineProperty, Indent]
        private I_FilterBuilder filter = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            I_Filter filter = this.filter.Build(owner, target, deliveryArguments);
            if (filter == null)
            {
                return null;
            }
            return new FilterComponent(filter, filterType);
        }
    }
}
using System;
using Manager;
using static Ashen.DeliverySystem.FilterComponentBuilder;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class FilterComponent : A_SimpleComponent
    {
        private FilterType filterType;
        private I_Filter filter;

        public FilterComponent() { }

        public FilterComponent(I_Filter filter, FilterType filterType)
        {
            this.filter = filter;
            this.filterType = filterType;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            KeyContainer<I_Filter> filterContainer = new KeyContainer<I_Filter>(filter, container.key);
            if (deliveryTool != null)
            {
                switch (filterType)
                {
                    case FilterType.PRE_DEFENSIVE:
                        deliveryTool.GetPreDefensiveFilters().Add(filterContainer);
                        break;
                    case FilterType.PRE_OFFENSIVE:
                        deliveryTool.GetPreOffensiveFilters().Add(filterContainer);
                        break;
                    case FilterType.POST_DEFENSIVE:
                        deliveryTool.GetPostDefensiveFilters().Add(filterContainer);
                        break;
                    case FilterType.POST_OFFENSIVE:
                        deliveryTool.GetPostOffensiveFilters().Add(filterContainer);
                        break;
                }
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            if (deliveryTool != null)
            {
                deliveryTool.RemoveFilter(filterType, container.key);
            }
        }

        
    }
}

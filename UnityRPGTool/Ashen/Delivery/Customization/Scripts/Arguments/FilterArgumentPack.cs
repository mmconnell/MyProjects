using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class FilterArgumentPack : A_DeliveryArgumentPack<FilterArgumentPack>
    {
        private List<KeyContainer<I_Filter>> preFilters;
        private List<KeyContainer<I_Filter>> postFilters;

        public override I_DeliveryArgumentPack Initialize()
        {
            return new FilterArgumentPack();
        }

        public void AddPreFilter(KeyContainer<I_Filter> filter)
        {
            if (preFilters == null)
            {
                preFilters = new List<KeyContainer<I_Filter>>();
            }
            preFilters.Add(filter);
        }

        public void AddPreFilters(List<KeyContainer<I_Filter>> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return;
            }
            if (preFilters == null)
            {
                preFilters = new List<KeyContainer<I_Filter>>();
            }
            preFilters.AddRange(filters);
        }

        public void AddPostFilter(KeyContainer<I_Filter> filter)
        {
            if (postFilters == null)
            {
                postFilters = new List<KeyContainer<I_Filter>>();
            }
            postFilters.Add(filter);
        }

        public void AddPostFilters(List<KeyContainer<I_Filter>> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return;
            }
            if (postFilters == null)
            {
                postFilters = new List<KeyContainer<I_Filter>>();
            }
            postFilters.AddRange(filters);
        }

        public List<KeyContainer<I_Filter>> GetPreFilters()
        {
            return preFilters;
        }

        public List<KeyContainer<I_Filter>> GetPostFilters()
        {
            return postFilters;
        }

        public override void Clear()
        {
            if (preFilters != null)
            {
                preFilters.Clear();
            }
            if (postFilters != null)
            {
                postFilters.Clear();
            }
        }
    }
}
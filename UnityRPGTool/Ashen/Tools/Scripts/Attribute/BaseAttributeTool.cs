using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using System;

namespace Manager
{
    public class BaseAttributeTool : A_EnumeratedTool<BaseAttributeTool>, I_Saveable
    {
        [ShowInInspector]
        private int[] attributeValues;
        private List<I_Cacheable>[] cacheables;

        [OdinSerialize]
        private BaseAttributeToolConfiguration baseAttributeToolConfiguration = default;
        private BaseAttributeToolConfiguration BaseAttributeToolConfiguration
        {
            get
            {
                if (baseAttributeToolConfiguration == null)
                {
                    return DefaultValues.Instance.defaultBaseAttributeToolConfiguration;
                }
                return baseAttributeToolConfiguration;
            }
        }

        public void Initialize(BaseAttributeToolConfiguration config)
        {
            baseAttributeToolConfiguration = config;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            int size = BaseAttributes.Count;
            attributeValues = new int[size];
            cacheables = new List<I_Cacheable>[size];
            for (int x = 0; x < size; x++)
            {
                attributeValues[x] = BaseAttributeToolConfiguration.DefaultBase[BaseAttributes.Instance[x]];
                cacheables[x] = new List<I_Cacheable>();
            }
        }

        public int GetAttribute(BaseAttribute attribute)
        {
            if (attributeValues == null)
            {
                return 0;
            }
            return attributeValues[(int)attribute];
        }

        [Button]
        public void AddBase(BaseAttribute attribute, int flat)
        { 
             attributeValues[(int)attribute] += flat;
             OnChange(attribute);
        }

        public void Cache(BaseAttribute attribute, I_Cacheable toCache)
        {
            if (!cacheables[(int)attribute].Contains(toCache))
            {
                cacheables[(int)attribute].Add(toCache);
            }
        }

        private void OnChange(BaseAttribute attribute)
        {
            foreach (I_Cacheable cacheable in cacheables[(int)attribute])
            {
                cacheable.Recalculate(toolManager.Get<DeliveryTool>(), null);
            }
        }

        public object CaptureState()
        {
            return new BaseAttributeSaveData
            {
                baseValues = this.attributeValues
            };
        }

        public void RestoreState(object state)
        {
            BaseAttributeSaveData saveData = (BaseAttributeSaveData)state;
            this.attributeValues = saveData.baseValues;
            foreach (BaseAttribute attrib in BaseAttributes.Instance)
            {
                OnChange(attrib);
            }
        }

        [Serializable]
        private struct BaseAttributeSaveData
        {
            public int[] baseValues;
        }
    }
}
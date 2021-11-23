using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Manager
{
    /**
     * This class is used to manage the various attributes of a Character (I.E. Strength, Dexerity, etc.)
     **/
    public class AttributeTool : A_EnumeratedTool<AttributeTool>, I_InvalidationListener
    {
        private ShiftableEquation[] attributes;
        [NonSerialized]
        private List<I_Cacheable>[] cacheables;
        [NonSerialized]
        private int[] values;
        [NonSerialized]
        private bool[] valid;
        private Dictionary<string, DerivedAttribute> keyToAttribute;

        [OdinSerialize]
        private AttributeToolConfiguration attributeToolConfiguration = default;
        private AttributeToolConfiguration AttributeToolConfiguration
        {
            get
            {
                if (attributeToolConfiguration == null)
                {
                    return DefaultValues.Instance.defaultAttributeToolConfiguration;
                }
                return attributeToolConfiguration;
            }
        }

        public void Initialize(AttributeToolConfiguration config)
        {
            attributeToolConfiguration = config;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            int size = DerivedAttributes.Count;
            attributes = new ShiftableEquation[size];
            cacheables = new List<I_Cacheable>[size];
            keyToAttribute = new Dictionary<string, DerivedAttribute>();
            valid = new bool[size];
            values = new int[size];
            for (int x = 0; x < size; x++)
            {
                ShiftableEquation equation = AttributeToolConfiguration.ShiftableEquation.Copy();
                attributes[x] = equation;
                equation.BaseValue = DerivedAttributes.Instance[x].equation;
                keyToAttribute.Add("AttributeTool-" + DerivedAttributes.Instance[x].ToString(), DerivedAttributes.Instance[x]);
                attributes[x] = equation;
                cacheables[x] = new List<I_Cacheable>();
            }
        }

        public void Start()
        {
            foreach (DerivedAttribute attribute in DerivedAttributes.Instance)
            {
                attribute.equation.AddInvalidationListener(toolManager.Get<DeliveryTool>(), this, "AttributeTool-" + attribute.ToString());
                GetAttribute(attribute);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void AddShift(DerivedAttribute characterAttribute, ShiftCategory shiftCategory, string source, int flat)
        {
            attributes[(int)characterAttribute].ApplyShift(shiftCategory, source, flat, 0f);
            OnChange(characterAttribute);
        }

        public void RemoveShift(DerivedAttribute characterAttribute, ShiftCategory shiftCategory, string source)
        {
            attributes[(int)characterAttribute].RemoveShift(shiftCategory, source);
            OnChange(characterAttribute);
        }

        public int GetAttribute(DerivedAttribute characterAttribute)
        {
            if (valid[(int)characterAttribute])
            {
                return values[(int)characterAttribute];
            }
            valid[(int)characterAttribute] = true;
            values[(int)characterAttribute] = (int)attributes[(int)characterAttribute].GetValue(toolManager.Get<DeliveryTool>(), null);
            return values[(int)characterAttribute];
        }

        public int GetBaseValue(DerivedAttribute attribute)
        {
            return (int)attributes[(int)attribute].GetBase(toolManager.Get<DeliveryTool>(), null);
        }

        public void Cache(DerivedAttribute attribute, I_Cacheable toCache)
        {
            if (!cacheables[(int)attribute].Contains(toCache))
            {
                cacheables[(int)attribute].Add(toCache);
            }
        }

        public void OnChange(DerivedAttribute attribute)
        {
            valid[(int)attribute] = false;
            GetAttribute(attribute);
            foreach (I_Cacheable cacheable in cacheables[(int)attribute])
            {
                cacheable.Recalculate(toolManager.Get<DeliveryTool>(), null);
            }
        }

        public void Invalidate(I_DeliveryTool toolManager, string key)
        {
            DerivedAttribute attribute = keyToAttribute[key];
            OnChange(attribute);
        }

        [ReadOnly, ShowInInspector]
        private Dictionary<DerivedAttribute, int> statValues = default;

        [Button]
        private void BuildStatValues()
        {
            if (DerivedAttributes.Instance == null)
            {
                statValues = null;
                return;
            }
            if (statValues == null || statValues.Count != DerivedAttributes.Count)
            {
                statValues = new Dictionary<DerivedAttribute, int>();
                foreach (DerivedAttribute sa in DerivedAttributes.Instance)
                {
                    statValues.Add(sa, 0);
                }
            }
            if (values != null && values.Length == DerivedAttributes.Count)
            {
                foreach (DerivedAttribute sa in DerivedAttributes.Instance)
                {
                    statValues[sa] = values[(int)sa];
                }
            }
        }
    }
}

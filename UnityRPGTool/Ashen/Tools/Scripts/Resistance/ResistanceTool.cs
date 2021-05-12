using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Manager
{
    /**
     * The ResistanceTool will manage all the resistances to each damage type that a character can receive
     **/
    public class ResistanceTool : A_EnumeratedTool<ResistanceTool>, I_InvalidationListener
    {
        [OdinSerialize]
        private ResistanceToolConfiguration resistanceToolConfiguration = default;
        private ResistanceToolConfiguration ResistanceToolConfiguration
        {
            get
            {
                if (resistanceToolConfiguration == null)
                {
                    return DefaultValues.Instance.defaultResistanceToolConfiguration;
                }
                return resistanceToolConfiguration;
            }
        }
        
        private ShiftableEquation[] resistances;
        private List<I_Cacheable>[] cacheables;

        private int[] values;
        private bool[] valid;
        private Dictionary<string, DamageType> keyToDamageType;

        public void Initialize(ResistanceToolConfiguration config)
        {
            resistanceToolConfiguration = config;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            int size = DamageTypes.Count;
            resistances = new ShiftableEquation[size];
            cacheables = new List<I_Cacheable>[size];
            values = new int[size];
            valid = new bool[size];
            keyToDamageType = new Dictionary<string, DamageType>();
            ShiftableEquation baseEquation = ResistanceToolConfiguration.ShiftableEquation;
            for (int x = 0; x < size; x++)
            {
                ShiftableEquation equation = ResistanceToolConfiguration.ShiftableEquation.Copy();
                resistances[x] = equation;
                equation.BaseValue = ResistanceToolConfiguration.ResistanceEquations[DamageTypes.Instance[x]].equation;
                cacheables[x] = new List<I_Cacheable>();
            }
        }

        public void Start()
        {
            foreach (DamageType damageType in DamageTypes.Instance)
            {
                resistances[(int)damageType].BaseValue.AddInvalidationListener(toolManager.Get<DeliveryTool>(), this, "ResistanceTool-" + damageType.ToString());
                GetResistance(damageType, null);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        } 

        public void AddShift(DamageType damageType, ShiftCategory shiftCategory, string source, int flat)
        {
            resistances[(int)damageType].ApplyShift(shiftCategory, source, flat, 0f);
            OnChange(damageType);
        }

        public void RemoveShift(DamageType damageType, ShiftCategory shiftCategory, string source)
        {
            resistances[(int)damageType].RemoveShift(shiftCategory, source);
            OnChange(damageType);
        }

        public int GetResistance(DamageType damageType, EquationArgumentPack extraArguments)
        {
            int damageTypeNum = (int)damageType;
            if (valid[damageTypeNum])
            {
                return values[damageTypeNum];
            }
            valid[damageTypeNum] = true;
            values[damageTypeNum] = (int)resistances[damageTypeNum].GetValue(toolManager.Get<DeliveryTool>(), extraArguments);
            return values[damageTypeNum];
        }

        public float GetResistancePercentage(DamageType damageType, EquationArgumentPack extraArguments)
        {
            float resistance = GetResistance(damageType, extraArguments);
            float resistancePercentage = 0f;
            resistancePercentage = (100f - resistance)/100f;
            return resistancePercentage;
        }

        public void OnChange(DamageType damageType)
        {
            valid[(int)damageType] = false;
            GetResistance(damageType, null);
            foreach (I_Cacheable cacheable in cacheables[(int)damageType])
            {
                cacheable.Recalculate(toolManager.Get<DeliveryTool>(), null);
            }
        }

        public void Cache(DerivedAttribute attribute, I_Cacheable toCache)
        {
            if (!cacheables[(int)attribute].Contains(toCache))
            {
                cacheables[(int)attribute].Add(toCache);
            }
        }

        public void Invalidate(I_DeliveryTool toolManager, string key)
        {
            DamageType damageType = keyToDamageType[key];
            OnChange(damageType);
        }

        /*[ReadOnly, ShowInInspector]
        private Dictionary<DamageType, int> resistanceValues = new Dictionary<DamageType, int>();
        
        [Button]
        private void BuildResistanceValues()
        {
            if (DamageTypes.Instance == null)
            {
                resistanceValues = null;
                return;
            }
            resistanceValues = new Dictionary<DamageType, int>();
            foreach (DamageType dt in DamageTypes.Instance)
            {
                resistanceValues.Add(dt, 0);
            }
            foreach (DamageType dt in DamageTypes.Instance)
            {
                if (values != null && values.Length == DamageTypes.Instance.Count)
                {
                    resistanceValues[dt] = values[(int)dt];
                }
            }
        }*/
    }
}

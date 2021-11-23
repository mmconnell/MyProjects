using Ashen.DeliverySystem;
using System.Collections.Generic;
using System;
using Ashen.EquationSystem;

namespace Manager
{ 
    public class ResourceValueTool : A_EnumeratedTool<ResourceValueTool>, I_Saveable
    {
        private DamageTool damageTool;
        private A_ThresholdValue[] thresholdValues;
        private List<I_ThresholdListener>[] toListen;

        private ResourceValue abilityResourceValue;
        public ResourceValue AbilityResourceValue
        {
            get
            {
                return abilityResourceValue;
            }
        }

        private ResourceValueToolConfiguration defaultResourceValueToolConfiguration;
        private ResourceValueToolConfiguration DefaultResourceValueToolConfiguration
        {
            get
            {
                if (!defaultResourceValueToolConfiguration)
                {
                    return DefaultValues.Instance.defaultResourceValueToolConfiguration;
                }
                return defaultResourceValueToolConfiguration;
            }
        }

        public void Initialize(ResourceValueToolConfiguration resourceValueToolConfiguration)
        {
            this.defaultResourceValueToolConfiguration = resourceValueToolConfiguration;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            thresholdValues = new A_ThresholdValue[ResourceValues.Count];
            toListen = new List<I_ThresholdListener>[ResourceValues.Count];
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                toListen[(int)resourceValue] = new List<I_ThresholdListener>();
                thresholdValues[(int)resourceValue] = resourceValue.threshold.BuildThresholdValue(toolManager, resourceValue);
            }
            abilityResourceValue = DefaultResourceValueToolConfiguration.DefaultAbilityResource;
        }

        private void Start()
        {
            damageTool = toolManager.Get<DamageTool>();
            if (damageTool)
            {
                foreach (ResourceValue resourceValue in ResourceValues.Instance)
                {
                    foreach (DamageType damageType in resourceValue.listenOn)
                    {
                        damageTool.RegisterListener(damageType, thresholdValues[(int)resourceValue]);
                    }
                    
                }
            }
            foreach (A_ThresholdValue value in thresholdValues)
            {
                value.Init(toolManager.Get<DeliveryTool>());
            }
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                foreach (I_ThresholdListener listener in toListen[(int)resourceValue])
                {
                    thresholdValues[(int)resourceValue].RegiserThresholdChangeListener(listener);
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (damageTool)
            {
                foreach (ResourceValue resourceValue in ResourceValues.Instance)
                {
                    foreach (DamageType damageType in resourceValue.listenOn)
                    {
                        damageTool.UnRegisterListener(damageType, thresholdValues[(int)resourceValue]);
                    }
                }
            }
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                foreach (I_ThresholdListener listener in toListen[(int)resourceValue])
                {
                    thresholdValues[(int)resourceValue].UnRegesterThresholdChangeListener(listener);
                }
            }
        }

        public void ApplyAmount(ResourceValue resourceValue, int total)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.ApplyAmount(total);
        }

        public void RemoveAmount(ResourceValue resourceValue, int total)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.RemoveAmount(total);
        }

        public ThresholdEventValue GetValue(ResourceValue resourceValue)
        {
            return thresholdValues[(int)resourceValue].GetCurrentState();
        }

        public void RegisterThresholdMetListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].RegisterThresholdMetListener(listener);
        }

        public void UnRegesterThresholdMetListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].UnRegesterThresholdMetListener(listener);
        }

        public void RegiserThresholdChangeListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].RegiserThresholdChangeListener(listener);
            DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
            thresholdValues[(int)resourceValue].Recalculate(toolManager.Get<DeliveryTool>(), packs.GetPack<EquationArgumentPack>());
            packs.Disable();
        }

        public void UnRegesterThresholdChangeListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].UnRegesterThresholdChangeListener(listener);
        }

        public float GetCurrentPercentage(ResourceValue resourceValue)
        {
            return thresholdValues[(int)resourceValue].Percentage();
        }

        public void DisableThresholdValue(ResourceValue resourceValue)
        {
            thresholdValues[(int)resourceValue].Disable();
        }

        public void EnableThresholdValue(ResourceValue resourceValue)
        {
            thresholdValues[(int)resourceValue].Enable();
        }

        public void ClearThresholdValue(ResourceValue resourceValue)
        {
            thresholdValues[(int)resourceValue].ClearDamage();
        }

        public object CaptureState()
        {
            ThresholdValueSaveData[] thresholdDatas = new ThresholdValueSaveData[thresholdValues.Length];
            ThresholdValueDecayManagerSaveData[] decayManagerDatas = new ThresholdValueDecayManagerSaveData[thresholdValues.Length];
            for (int x = 0; x < thresholdValues.Length; x++)
            {
                A_ThresholdValue value = thresholdValues[x];
                thresholdDatas[x] = new ThresholdValueSaveData
                {
                    currentValue = value.currentValue,
                    enabled = value.enabled,
                };
                I_ThresholdDecayManager manager = value.decayManager;
                if (manager is ThresholdDecayManager decayManager)
                {
                    decayManagerDatas[x] = new ThresholdValueDecayManagerSaveData
                    {
                        decay = decayManager.decay,
                        decayWait = decayManager.decayWait,
                    };
                }
                else
                {
                    decayManagerDatas[x] = new ThresholdValueDecayManagerSaveData { };
                }
            }

            return new ResourceValueSaveData
            {
                thresholdValues = thresholdDatas,
                decayManagerValues = decayManagerDatas,
            };
        }
        
        public void RestoreState(object state)
        {
            ResourceValueSaveData saveData = (ResourceValueSaveData)state;
            for (int x = 0; x < thresholdValues.Length; x++)
            {
                ThresholdValueSaveData thresholdValue = saveData.thresholdValues[x];
                ThresholdValueDecayManagerSaveData managerValue = saveData.decayManagerValues[x];
                A_ThresholdValue value = thresholdValues[x];
                value.Reset(thresholdValue.currentValue, thresholdValue.enabled);
                if (value.decayManager is ThresholdDecayManager manager)
                {
                    manager.Reset(managerValue.decay, managerValue.decayWait);
                }
            }
        }

        [Serializable]
        public struct ResourceValueSaveData
        {
            public ThresholdValueSaveData[] thresholdValues;
            public ThresholdValueDecayManagerSaveData[] decayManagerValues;
        }

        [Serializable]
        public struct ThresholdValueSaveData
        {
            public int currentValue;
            public bool enabled;
        }

        [Serializable]
        public struct ThresholdValueDecayManagerSaveData
        {
            public TimeTicker decay;
            public TimeTicker decayWait;
        }
    }
}
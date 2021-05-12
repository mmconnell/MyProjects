using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using System.Collections.Generic;
using Sirenix.Serialization;
using Manager;
using System;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    public abstract class A_ThresholdValue : I_Cacheable, I_DamageListener
    {
        public abstract void ClearDamage();
        public abstract void ApplyAmount(int damage);
        public abstract void RemoveAmount(int damage);
        public abstract bool IsThresholdMet();
        public abstract int GetBaseValue();

        public virtual void Init(I_DeliveryTool deliveryTool)
        {
            this.deliveryTool = deliveryTool;
            currentMaxValue = (int)maxValue.Get(deliveryTool);
            AttributeTool attributeTool = (deliveryTool as DeliveryTool).toolManager.Get<AttributeTool>();
            attributeTool.Cache(maxValue, this);
            if (decayManager != null)
            {
                decayManager.Init(this, deliveryTool);
            }
        }

        private List<I_ThresholdListener> thresholdMetListener;
        private List<I_ThresholdListener> thresholdChangeListener;
        
        protected DerivedAttribute maxValue;
        [HideInEditorMode, ShowInInspector, ReadOnly]
        protected int currentMaxValue;
        
        protected bool retainRatioOnMaxHigher;
        protected bool retainRatioOnMaxLower;

        public int currentValue;
        protected I_DeliveryTool deliveryTool;
        public bool enabled;
        public I_ThresholdDecayManager decayManager;

        public A_ThresholdValue(DerivedAttribute maxValue, I_ThresholdDecayManager manager, bool retainRatioOnHigher, bool retainRatioOnLower)
        {
            this.maxValue = maxValue;
            this.retainRatioOnMaxHigher = retainRatioOnHigher;
            this.retainRatioOnMaxLower = retainRatioOnLower;
            enabled = true;
            this.decayManager = manager;
            thresholdChangeListener = new List<I_ThresholdListener>();
            thresholdMetListener = new List<I_ThresholdListener>();
        }

        public ThresholdEventValue GetCurrentState()
        {
            int maxValue = (int)this.maxValue.Get(deliveryTool);
            return new ThresholdEventValue
            {
                currentValue = currentValue,
                max = currentValue >= currentMaxValue,
                maxValue = currentMaxValue,
                min = currentValue <= 0,
                previousValue = currentValue,
                damageTaken = false
            };
        }

        public void ReportChange(ThresholdEventValue value)
        {
            //Logger.DebugLog("Threshold Change Event for " + " firing");
            foreach (I_ThresholdListener listener in thresholdChangeListener)
            {
                listener.OnThresholdEvent(value);
            }
        }

        public float Percentage()
        {
            return currentValue / maxValue.Get(deliveryTool);
        }

        public void ReportThresholdMet(ThresholdEventValue value)
        {
            //Logger.DebugLog("Threshold Met Event for " + damageType.name + " firing");
            foreach (I_ThresholdListener listener in thresholdMetListener)
            {
                listener.OnThresholdEvent(value);
            }
        }

        public void RegisterThresholdMetListener(I_ThresholdListener listener)
        {
            thresholdMetListener.Add(listener);
        }

        public void UnRegesterThresholdMetListener(I_ThresholdListener listener)
        {
            thresholdMetListener.Remove(listener);
        }

        public void RegiserThresholdChangeListener(I_ThresholdListener listener)
        {
            thresholdChangeListener.Add(listener);
        }

        public void UnRegesterThresholdChangeListener(I_ThresholdListener listener)
        {
            thresholdChangeListener.Remove(listener);
        }

        public void Disable()
        {
            ClearDamage();
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Reset(int currentValue, bool enabled)
        {
            this.currentValue = currentValue;
            this.enabled = enabled;
            this.currentMaxValue = (int)maxValue.Get(deliveryTool);
            bool max = false;
            bool min = false;
            if (currentValue <= 0)
            {
                currentValue = 0;
                min = true;
            }
            if (currentValue >= currentMaxValue)
            {
                currentValue = currentMaxValue;
                max = true;
            }
            ThresholdEventValue value = new ThresholdEventValue
            {
                currentValue = currentValue,
                max = max,
                maxValue = currentMaxValue,
                min = min,
                previousValue = currentValue,
                damageTaken = false
            };
            ReportChange(value);
        }

        public void Recalculate(I_DeliveryTool deliveryTool, EquationArgumentPack extraArguments)
        {
            float oldPercentage = this.currentValue / (float)currentMaxValue;
            int newMaxValue = (int)maxValue.Get(this.deliveryTool);
            int currentValue = this.currentValue;
            if (newMaxValue > currentMaxValue)
            {
                currentValue = retainRatioOnMaxHigher ? ((int)(newMaxValue * oldPercentage)) : this.currentValue;
            }
            if (newMaxValue < currentMaxValue)
            {
                currentValue = retainRatioOnMaxLower ? ((int)(newMaxValue * oldPercentage)) : this.currentValue;
            }
            currentMaxValue = newMaxValue;
            bool max = false;
            bool min = false;
            if (currentValue <= 0)
            {
                currentValue = 0;
                min = true;
            }
            if (currentValue >= newMaxValue)
            {
                currentValue = newMaxValue;
                max = true;
            }
            ThresholdEventValue value = new ThresholdEventValue
            {
                currentValue = currentValue,
                max = max,
                maxValue = newMaxValue,
                min = min,
                previousValue = this.currentValue,
                damageTaken = false
            };
            this.currentValue = currentValue;
            if (IsThresholdMet())
            {
                ReportThresholdMet(value);
            }
            ReportChange(value);
        }

        public void OnDamageEvent(DamageEvent damageEvent)
        {
            ApplyAmount(damageEvent.damageAmount);
        }
    }
}
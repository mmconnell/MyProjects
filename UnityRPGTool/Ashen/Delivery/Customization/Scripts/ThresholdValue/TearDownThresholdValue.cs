using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Manager;

namespace Ashen.DeliverySystem
{
    public class TearDownThresholdValue : A_ThresholdValue
    {
        public TearDownThresholdValue(DerivedAttribute maxValue, ResourceValue resourceValue, I_ThresholdDecayManager manager, bool retainRatioOnHigher, bool retainRatioOnLower)
            : base(maxValue, resourceValue, manager, retainRatioOnHigher, retainRatioOnLower) { }

        public override void ClearDamage()
        {
            RemoveAmount(currentValue);
        }

        public override void Init(I_DeliveryTool deliveryTool)
        {
            base.Init(deliveryTool);
            currentValue = (int)maxValue.Get(deliveryTool);
        }

        public override bool IsThresholdMet()
        {
            return currentValue == 0;
        }

        public override void ApplyAmount(int damage)
        {
            if (!enabled)
            {
                return;
            }
            int previous = currentValue;
            currentValue -= damage;
            if (currentValue < 0)
            {
                currentValue = 0;
            }
            if (currentValue != previous)
            {
                ThresholdEventValue value = new ThresholdEventValue
                {
                    resourceValue = resourceValue,
                    currentValue = currentValue,
                    previousValue = previous,
                    maxValue = currentMaxValue,
                    min = (currentValue == 0),
                    max = (currentValue == currentMaxValue),
                    damageTaken = true
                };
                if (value.min)
                {
                    ReportThresholdMet(value);
                }
                ReportChange(value);
            }
        }

        public override void RemoveAmount(int damage)
        {
            if (!enabled)
            {
                return;
            }
            int previous = currentValue;
            currentValue += damage;
            if (currentValue > currentMaxValue)
            {
                currentValue = currentMaxValue;
            }
            if (currentValue != previous)
            {
                ThresholdEventValue value = new ThresholdEventValue
                {
                    resourceValue = resourceValue,
                    currentValue = currentValue,
                    previousValue = previous,
                    maxValue = currentMaxValue,
                    min = (currentValue == 0),
                    max = (currentValue == currentMaxValue),
                    damageTaken = false
                };
                if (value.min)
                {
                    ReportThresholdMet(value);
                }
                ReportChange(value);
            }
        }

        public override int GetBaseValue()
        {
            return currentMaxValue;
        }
    }
}
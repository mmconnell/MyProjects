using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Manager;

namespace Ashen.DeliverySystem
{
    public class ThresholdBuilder
    {
        [EnumToggleButtons, HideLabel]
        public ThresholdType thresholdType;
        [OdinSerialize]
        public DerivedAttribute maxValue;
        [ToggleGroup("hasDecay")]
        public bool hasDecay;
        [ToggleGroup("hasDecay"), HideLabel, Title("Delay")]
        public DerivedAttribute decayDelay;
        [ToggleGroup("hasDecay"), HideLabel, Title("Rate")]
        public DerivedAttribute decayRate;
        [FoldoutGroup("Advanced")]
        public bool retainRatioOnMaxHigher;
        [FoldoutGroup("Advanced")]
        public bool retainRatioOnMaxLower;

        public A_ThresholdValue BuildThresholdValue(ToolManager toolManager)
        {
            A_ThresholdValue value = null;
            ThresholdDecayManager manager = null;
            if (hasDecay && !TimeRegistry.Instance.turnBased)
            {
                manager = new ThresholdDecayManager(decayDelay, decayRate);
            }
            switch (thresholdType)
            {
                case ThresholdType.BUILD_UP:
                    value = new BuildUpThresholdValue(
                        maxValue, 
                        manager,
                        retainRatioOnMaxHigher, 
                        retainRatioOnMaxLower);
                    break;
                case ThresholdType.TEAR_DOWN:
                    value = new TearDownThresholdValue(
                        maxValue, 
                        manager, 
                        retainRatioOnMaxHigher, 
                        retainRatioOnMaxLower);
                    break;
            }
            if (value == null)
            {
                Logger.ErrorLog("No threshold value type was configured for threshold builder");
            }
            return value;
        }
    }

    public enum ThresholdType
    {
        BUILD_UP, TEAR_DOWN
    }
}
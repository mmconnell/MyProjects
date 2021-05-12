using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class LevelTool : A_EnumeratedTool<LevelTool>
    {
        [ShowInInspector, ReadOnly]
        private A_ThresholdValue experienceThresholdValue = default;

        [OdinSerialize]
        private LevelToolConfiguration levelToolConfiguration = default;

        private LevelToolConfiguration LevelToolConfiguration
        {
            get
            {
                if (levelToolConfiguration == null)
                {
                    return DefaultValues.Instance.defaultLevelToolConfiguration;
                }
                return levelToolConfiguration;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            experienceThresholdValue = LevelToolConfiguration.ExperienceThresholdValue;
        }

        public void Start()
        {
            experienceThresholdValue.Init(toolManager.Get<DeliveryTool>());
        }

        public void ApplyAmount(int amount)
        {
            experienceThresholdValue.ApplyAmount(amount);
        }

        public void RemoveAmount(int amount)
        {
            experienceThresholdValue.RemoveAmount(amount);
        }

        public void RegisterThresholdMetListener(I_ThresholdListener listener)
        {
            experienceThresholdValue.RegisterThresholdMetListener(listener);
        }

        public void UnRegesterThresholdMetListener(I_ThresholdListener listener)
        {
            experienceThresholdValue.UnRegesterThresholdMetListener(listener);
        }

        public void RegiserThresholdChangeListener(I_ThresholdListener listener)
        {
            experienceThresholdValue.RegiserThresholdChangeListener(listener);
        }

        public void UnRegesterThresholdChangeListener(I_ThresholdListener listener)
        {
            experienceThresholdValue.UnRegesterThresholdChangeListener(listener);
        }

        public float GetCurrentPercentage()
        {
            return experienceThresholdValue.Percentage();
        }

        public void DisableThresholdValue()
        {
            experienceThresholdValue.Disable();
        }

        public void EnableThresholdValue()
        {
            experienceThresholdValue.Enable();
        }

        public void ClearThresholdValue()
        {
            experienceThresholdValue.ClearDamage();
        }
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Ashen.DeliverySystem;

[CreateAssetMenu(fileName = "LevelToolConfiguration", menuName = "Custom/Tool/LevelToolConfiguration")]
public class LevelToolConfiguration : SerializedScriptableObject
{
    [OdinSerialize]
    private A_ThresholdValue experienceThresholdValue = default;

    public A_ThresholdValue ExperienceThresholdValue
    {
        get
        {
            if (experienceThresholdValue != null)
            {
                //return experienceThresholdValue.Clone();
            }
            else
            {
                if (this != DefaultValues.Instance.defaultLevelToolConfiguration)
                {
                    //return DefaultValues.Instance.defaultLevelToolConfiguration.experienceThresholdValue.Clone();
                }
            }
            return null;
        }
    }
}
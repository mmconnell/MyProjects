using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Sirenix.Serialization;
using Manager;

[CreateAssetMenu(fileName = "StatusToolConfiguration", menuName = "Custom/Tool/StatusToolConfiguration")]
public class StatusToolConfiguration : SerializedScriptableObject
{
    [OdinSerialize]
    private Dictionary<ResourceValue, DeliveryPackScriptableObject> statusEffectResultMap = default;
    
    public DeliveryPackScriptableObject[] StatusEffectResults
    {
        get
        {
            if (statusEffectResultMap == null)
            {
                if (this == DefaultValues.Instance.defaultStatusToolConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultStatusToolConfiguration.StatusEffectResults;
            }
            else
            {
                DeliveryPackScriptableObject[]  derivedStatusEffectResults = new DeliveryPackScriptableObject[ResourceValues.Count];
                foreach (ResourceValue resourceValue in ResourceValues.Instance)
                {
                    if (statusEffectResultMap.ContainsKey(resourceValue))
                    {
                        derivedStatusEffectResults[(int)resourceValue] = statusEffectResultMap[resourceValue];
                    }
                    else
                    {
                        if (this != DefaultValues.Instance.defaultStatusToolConfiguration)
                        {
                            derivedStatusEffectResults[(int)resourceValue] = DefaultValues.Instance.defaultStatusToolConfiguration.StatusEffectResults[(int)resourceValue];
                        }
                    }
                }
                return derivedStatusEffectResults;
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "BaseAttributeToolConfiguration", menuName = "Custom/Tool/BaseAttributeToolConfiguration")]
public class BaseAttributeToolConfiguration : SerializedScriptableObject
{
    [OdinSerialize]
    private Dictionary<BaseAttribute, int> defaultBase = default;
    
    public Dictionary<BaseAttribute, int> DefaultBase
    {
        get
        {
            if (defaultBase == null)
            {
                if (this == DefaultValues.Instance.defaultBaseAttributeToolConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultBaseAttributeToolConfiguration.DefaultBase;
            }
            else
            {
                Dictionary<BaseAttribute, int> derivedDefaultBase = new Dictionary<BaseAttribute, int>();
                foreach (BaseAttribute statAttribute in BaseAttributes.Instance)
                {
                    if (defaultBase.ContainsKey(statAttribute))
                    {
                        derivedDefaultBase.Add(statAttribute, defaultBase[statAttribute]);
                    }
                    else
                    {
                        if (this != DefaultValues.Instance.defaultBaseAttributeToolConfiguration)
                        {
                            derivedDefaultBase.Add(statAttribute, DefaultValues.Instance.defaultBaseAttributeToolConfiguration.defaultBase[statAttribute]);
                        }
                    }
                }
                return derivedDefaultBase;
            }
        } 
    }
}

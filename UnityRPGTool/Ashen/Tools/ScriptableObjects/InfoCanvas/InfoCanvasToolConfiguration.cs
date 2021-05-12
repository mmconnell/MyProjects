using Ashen.DeliverySystem;
using Manager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoCanvasToolConfiguration", menuName = "Custom/Tool/InfoCanvasToolConfiguration")]
public class InfoCanvasToolConfiguration : SerializedScriptableObject
{
    [OdinSerialize]
    private Dictionary<DamageType, BarInfo> barConfigurations = default;

    [OdinSerialize]
    private bool useBarInitialValue = default;
    [OdinSerialize, ShowIf("useBarInitialValue")]
    private float barInitialValue = default;
    public float BarInitialValue
    {
        get
        {
            if (useBarInitialValue)
            {
                return barInitialValue;
            }
            return DefaultValues.Instance.defaultInfoCanvasToolConfiguration.barInitialValue;
        }
    }

    [OdinSerialize]
    private bool useBarGrowthValue = default;

    [OdinSerialize, ShowIf("useBarGrowthValue")]
    private float barGrowthValue = default;
    public float BarGrowthValue
    {
        get
        {
            if (useBarGrowthValue)
            {
                return barGrowthValue;
            }
            return DefaultValues.Instance.defaultInfoCanvasToolConfiguration.barGrowthValue;
        }
    }
    
    public BarInfo[] DerivedBarConfigurations
    {
        get
        {
            if (barConfigurations == null)
            {
                return DefaultValues.Instance.defaultInfoCanvasToolConfiguration.DerivedBarConfigurations;
            }
            else
            {
                BarInfo[]  derivedBarConfigurations = new BarInfo[DamageTypes.Count];
                foreach (DamageType damageType in DamageTypes.Instance)
                {
                    if (barConfigurations.ContainsKey(damageType))
                    {
                        derivedBarConfigurations[(int)damageType] = barConfigurations[damageType];
                    }
                    else
                    {
                        if (this != DefaultValues.Instance.defaultInfoCanvasToolConfiguration)
                        {
                            derivedBarConfigurations[(int)damageType] = DefaultValues.Instance.defaultInfoCanvasToolConfiguration.barConfigurations[damageType];
                        }
                    }
                }
                return derivedBarConfigurations;
            }
        }
    }

    [OdinSerialize]
    private List<ResourceValue> defaultResourceValues = default;
    public List<ResourceValue> DefaultResourceValues
    {
        get
        {
            if (DefaultValues.Instance.defaultInfoCanvasToolConfiguration == this)
            {
                return defaultResourceValues;
            }
            return DefaultValues.Instance.defaultInfoCanvasToolConfiguration.defaultResourceValues;
        }
    }

    [OdinSerialize]
    private bool useSymbolInitialValue = default;
    [OdinSerialize, ShowIf("useSymbolInitialValue")]
    private float symbolInitialValue = default;
    public float SymbolInitialValue
    {
        get
        {
            if (useSymbolInitialValue)
            {
                return symbolInitialValue;
            }
            return DefaultValues.Instance.defaultInfoCanvasToolConfiguration.symbolInitialValue;
        }
    }

    [OdinSerialize]
    private bool useSymbolGrowthValue = default;

    [OdinSerialize, ShowIf("useSymbolGrowthValue")]
    private float symbolGrowthValue = default;
    public float SymbolGrowthValue
    {
        get
        {
            if (useSymbolGrowthValue)
            {
                return symbolGrowthValue;
            }
            return DefaultValues.Instance.defaultInfoCanvasToolConfiguration.symbolGrowthValue;
        }
    }
}
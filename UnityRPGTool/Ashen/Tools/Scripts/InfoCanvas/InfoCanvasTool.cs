using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Manager;
using Sirenix.Serialization;
using Ashen.DeliverySystem;

public class InfoCanvasTool : A_EnumeratedTool<InfoCanvasTool>
{
    [OdinSerialize]
    private InfoCanvasToolConfiguration infoCanvasToolConfiguration = default;
    private InfoCanvasToolConfiguration InfoCanvasToolConfiguration
    {
        get
        {
            if (infoCanvasToolConfiguration == null)
            {
                return DefaultValues.Instance.defaultInfoCanvasToolConfiguration;
            }
            return infoCanvasToolConfiguration;
        }
    }
    public BarCanvasManager barCanvasManager;
    public StatusEffectSymbolsManager statusEffectSymbolsManager;

    public override void Initialize()
    {
        base.Initialize();
        List<ResourceValue> defaultResourceValues = InfoCanvasToolConfiguration.DefaultResourceValues;
        BarInfo[] barInfos = InfoCanvasToolConfiguration.DerivedBarConfigurations;
        barCanvasManager.defaultResourceValues = defaultResourceValues;
        barCanvasManager.barInfos = barInfos;
        barCanvasManager.initialValue = InfoCanvasToolConfiguration.BarInitialValue;
        barCanvasManager.growthRate = InfoCanvasToolConfiguration.BarGrowthValue;
        barCanvasManager.toolManager = toolManager;

        statusEffectSymbolsManager.initialValue = InfoCanvasToolConfiguration.SymbolInitialValue;
        statusEffectSymbolsManager.growthRate = InfoCanvasToolConfiguration.SymbolGrowthValue;
    }
}

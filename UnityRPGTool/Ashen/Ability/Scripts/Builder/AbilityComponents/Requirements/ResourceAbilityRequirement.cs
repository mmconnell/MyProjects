using Manager;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;
using Ashen.DeliverySystem;
using System;

[Serializable]
public class ResourceAbilityRequirement : I_AbilityRequirement
{
    [EnumToggleButtons, HideLabel]
    public CostTypeInspector costType;

    [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
    [ShowIf(nameof(costType), CostTypeInspector.PrimaryResource)]
    public I_Equation primaryResourceValue;

    [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
    [ShowIf(nameof(costType), CostTypeInspector.Health)]
    public I_Equation healthValue;

    [ShowIf(nameof(costType), CostTypeInspector.Custom), Hide]
    public AbilityRequirementsCostCustom customRequirements;

    public bool IsValid(ToolManager toolManager)
    {
        ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
        DeliveryTool dTool = toolManager.Get<DeliveryTool>();
        ThresholdEventValue curValue = default;
        int reqValue = 0;
        switch(costType)
        {
            case CostTypeInspector.PrimaryResource:
                curValue = rvTool.GetValue(rvTool.AbilityResourceValue);
                reqValue = (int)primaryResourceValue.Calculate(dTool);
                break;
            case CostTypeInspector.Health:
                curValue = rvTool.GetValue(ResourceValues.Instance.health);
                reqValue = (int)healthValue.Calculate(dTool);
                break;
            case CostTypeInspector.Custom:
                curValue = rvTool.GetValue(customRequirements.resourceValue);
                reqValue = (int)customRequirements.customResourceValue.Calculate(dTool);
                break;
        }
        return reqValue <= curValue.currentValue;
    }
}

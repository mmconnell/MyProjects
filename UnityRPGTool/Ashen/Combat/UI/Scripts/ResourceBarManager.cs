using Ashen.DeliverySystem;
using JoshH.UI;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarManager : MonoBehaviour, I_ThresholdListener
{
    private ToolManager toolManager;

    public TextMeshProUGUI resourceAmountText;
    public TextMeshProUGUI resourceNameText;
    public Slider resourceSlider;
    public UIGradient gradient;

    private ResourceValue registeredValue;

    public void OnThresholdEvent(ThresholdEventValue value)
    {
        if (resourceAmountText)
        {
            resourceAmountText.text = value.currentValue + "";
        }
        resourceSlider.value = value.currentValue / ((float)value.maxValue);
    }

    public void RegisterToolManager(ToolManager toolManager, ResourceValue resourceValue)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
        resourceValueTool.RegiserThresholdChangeListener(resourceValue, this);
        if (resourceNameText)
        {
            resourceNameText.text = resourceValue.displayName;
        }
        if (gradient)
        {
            gradient.LinearColor1 = resourceValue.color1;
            gradient.LinearColor2 = resourceValue.color2;
        }
        registeredValue = resourceValue;
    }

    public void UnregisterToolManager()
    {
        if (this.toolManager)
        {
            ResourceValueTool oldValueTool = this.toolManager.Get<ResourceValueTool>();
            oldValueTool.UnRegesterThresholdChangeListener(registeredValue, this);
            registeredValue = null;
            toolManager = null;
        }
    }
}

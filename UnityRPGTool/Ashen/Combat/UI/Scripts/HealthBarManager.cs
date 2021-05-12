using Ashen.DeliverySystem;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour, I_ThresholdListener
{
    public ToolManager toolManager;
    private float lastWidth = 0f;
    public float spacing = 0.5f;
    public float defaultWidth = 5f;
    public GameObject healthUnit;

    public Color disabledHealth;

    public bool autoconfigured = false;

    HorizontalOrVerticalLayoutGroup layoutGroup;
    RectTransform rectTransform;

    public void OnThresholdEvent(ThresholdEventValue value)
    {
        if (layoutGroup == null || rectTransform == null)
        {
            return;
        }
        layoutGroup.spacing = this.spacing;
        float spacing = layoutGroup.padding.left;
        spacing += layoutGroup.padding.right;
        int childCount = value.maxValue;
        int count = 0;
        float currentWidth = rectTransform.rect.width;
        lastWidth = currentWidth;
        List<RectTransform> childRect = new List<RectTransform>();
        List<GameObject> toDestroy = new List<GameObject>();
        foreach (Transform child in transform)
        {
            count++;
            if (count > childCount)
            {
                toDestroy.Add(child.gameObject);
            }
            else
            {
                childRect.Add(child.gameObject.GetComponent<RectTransform>());
            }
        }
        while (count < childCount)
        {
            GameObject child = Instantiate(healthUnit, transform);
            childRect.Add(child.GetComponent<RectTransform>());
            count++;
        }
        for (int x = 0; x < toDestroy.Count; x++)
        {
            DestroyImmediate(toDestroy[x]);
        }
        float totalWidth = (defaultWidth * childCount);
        float betweenSpacing = (childCount - 1) * this.spacing;

        float availableSpace = currentWidth - betweenSpacing - spacing;
        float newWidth = availableSpace / childCount;
        if (newWidth > this.defaultWidth)
        {
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childControlWidth = false;
            foreach (RectTransform rect in childRect)
            {
                rect.sizeDelta = new Vector2(defaultWidth, rect.sizeDelta.y);
            }
        }
        else
        {
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childControlWidth = true;
        }
        int healthCount = 0;
        foreach (RectTransform child in childRect)
        {
            Image image = child.GetComponent<Image>();
            if (healthCount < value.currentValue)
            {
                image.color = Color.red;
            }
            else
            {
                image.color = disabledHealth;
            }
            healthCount++;
        }
    }

    private void Update()
    {
        if (!toolManager)
        {
            return;
        }
        if (toolManager != null && !autoconfigured)
        {
            ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
            resourceValueTool.RegiserThresholdChangeListener(ResourceValues.Instance.health, this);
            autoconfigured = true;
        }
        float lastWidth = rectTransform.rect.width;
        if (this.lastWidth != lastWidth)
        {
            ThresholdEventValue value = toolManager.Get<ResourceValueTool>().GetValue(ResourceValues.Instance.health);
            OnThresholdEvent(value);
        }
    }

    public void RegisterToolManager(ToolManager toolManager)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
        resourceValueTool.RegiserThresholdChangeListener(ResourceValues.Instance.health, this);
        autoconfigured = true;
    }

    public void UnregisterToolManager()
    {
        if (this.toolManager)
        {
            ResourceValueTool oldValueTool = this.toolManager.Get<ResourceValueTool>();
            oldValueTool.UnRegesterThresholdChangeListener(ResourceValues.Instance.health, this);
            autoconfigured = false;
        }
    }

    private void Start()
    {
        layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        
    }
}

using Ashen.DeliverySystem;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarManager : MonoBehaviour, I_ThresholdListener
{
    public ResourceValue resourceValue;
    public float spacing = 0.5f;
    public float defaultWidth = 5f;
    HorizontalOrVerticalLayoutGroup layoutGroup;
    RectTransform rectTransform;
    public GameObject resourceUnit;
    public Color disabledResource;

    private float lastWidth;
    private ToolManager toolManager;

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
            GameObject child = Instantiate(resourceUnit, transform);
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
        int resourceCount = 0;
        foreach (RectTransform child in childRect)
        {
            Image image = child.GetComponent<Image>();
            if (resourceCount < value.currentValue)
            {
                image.color = Color.yellow;
            }
            else
            {
                image.color = disabledResource;
            }
            resourceCount++;
        }
    }

    private void Update()
    {
        if (!toolManager)
        {
            return;
        }
        float lastWidth = rectTransform.rect.width;
        if (this.lastWidth != lastWidth)
        {
            ThresholdEventValue value = toolManager.Get<ResourceValueTool>().GetValue(resourceValue);
            OnThresholdEvent(value);
        }
    }

    public void RegisterToolManager(ToolManager toolManager)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
        resourceValueTool.RegiserThresholdChangeListener(resourceValue, this);
    }

    public void UnregisterToolManager()
    {
        if (this.toolManager)
        {
            ResourceValueTool oldValueTool = this.toolManager.Get<ResourceValueTool>();
            oldValueTool.UnRegesterThresholdChangeListener(resourceValue, this);
        }
    }

    private void Start()
    {
        layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
}

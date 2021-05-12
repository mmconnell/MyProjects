using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ArmorBarManager : MonoBehaviour
{
    public float defaultSpacing = 0.5f;
    HorizontalOrVerticalLayoutGroup layoutGroup;
    RectTransform rectTransform;
    private float width = 0f;

    private void Start()
    {
        layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        
    }
    // Update is called once per frame
    void Update()
    {
        float heightSpacing = layoutGroup.padding.top;
        heightSpacing += layoutGroup.padding.bottom;
        width = rectTransform.sizeDelta.y - heightSpacing;
        layoutGroup.spacing = this.defaultSpacing;
        float spacing = layoutGroup.padding.left;
        spacing += layoutGroup.padding.right;
        int childCount = 0;
        foreach (Transform child in transform)
        {
            childCount++;
        }
        float totalWidth = (width * childCount);
        //float betweenSpacing = (childCount - 1) * this.defaultSpacing;

        float availableSpace = rectTransform.rect.width - totalWidth - spacing;

        float newSpacing = availableSpace / (childCount - 1);
        if (newSpacing > this.defaultSpacing)
        {
            this.layoutGroup.spacing = defaultSpacing;
        }
        else
        {
            this.layoutGroup.spacing = newSpacing;
        }
    }
}

using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttributeUI : A_AttributeUI
{
    public BaseAttribute baseAttribute;
    public DerivedAttribute derivedAttribute;

    private BaseAttributeTool baseAttributeTool;
    private AttributeTool attributeTool;

    public override int GetValue()
    {
        return attributeTool.GetAttribute(derivedAttribute);
    }

    public override string GetDefaultName()
    {
        return baseAttribute.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        baseAttributeTool = binder.boundTool.Get<BaseAttributeTool>();
        attributeTool = binder.boundTool.Get<AttributeTool>();
        tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();
        baseAttributeTool.Cache(baseAttribute, this);
        SetText();
    }

    public override int GetBaseValue()
    {
        return attributeTool.GetBaseValue(derivedAttribute);
    }
}

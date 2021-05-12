using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerivedAttributeUI : A_AttributeUI
{
    public DerivedAttribute derivedAttribute;

    private AttributeTool attributeTool;

    public override int GetValue()
    {
        return attributeTool.GetAttribute(derivedAttribute);
    }

    public override string GetDefaultName()
    {
        return derivedAttribute.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        attributeTool = binder.boundTool.Get<AttributeTool>();
        attributeTool.Cache(derivedAttribute, this);
        tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();
        SetText();
    }

    public override int GetBaseValue()
    {
        return attributeTool.GetBaseValue(derivedAttribute);
    }
}

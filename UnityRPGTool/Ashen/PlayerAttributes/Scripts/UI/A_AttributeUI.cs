using System.Collections;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using TMPro;
using UnityEngine;

public abstract class A_AttributeUI : MonoBehaviour, I_Cacheable
{
    public CharacterBinderUI binder;
    public TextMeshProUGUI text;
    public string overrideName;
    public TooltipTrigger tooltipTrigger;

    public void Recalculate(I_DeliveryTool toolManager, EquationArgumentPack extraArguments)
    {
        SetText();
    }

    public void SetText()
    {
        string name = "";
        if (overrideName != null && overrideName != "")
        {
            name += overrideName;
        }
        else
        {
            name += GetDefaultName();
        }
        text.text = name + ": " + GetValue();
        SetTooltip();
    }

    public void SetTooltip()
    {
        tooltipTrigger.content = "Base: " + GetBaseValue() + "\nBonus: " + GetValue();
    }

    public abstract string GetDefaultName();
    public abstract int GetValue();
    public abstract int GetBaseValue();
}

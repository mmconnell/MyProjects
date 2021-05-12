using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Manager;

public abstract class A_PartySelector : Selectable, I_Targetable, ISubmitHandler, ICancelHandler
{
    public List<RowHandler> rowHandlers;

    public void Deselected()
    {
        foreach (RowHandler row in rowHandlers)
        {
            row.Deselected();
        }
    }

    public void Selected()
    {
        foreach (RowHandler row in rowHandlers)
        {
            row.Selected();
        }
    }

    public abstract void OnSubmit(BaseEventData eventData);

    public override void OnSelect(BaseEventData eventData)
    {
        Selected();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        Deselected();
    }

    public Selectable GetSelectableObject()
    {
        return this;
    }

    public List<ToolManager> GetTargets()
    {
        List<ToolManager> toolManagers = new List<ToolManager>();
        foreach (RowHandler row in rowHandlers)
        {
            toolManagers.AddRange(row.GetTargets());
        }
        return toolManagers;
    }

    public void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }
}

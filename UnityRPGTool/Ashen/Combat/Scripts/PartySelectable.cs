using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Manager;

public class PartySelectable : Selectable, I_Targetable, ISubmitHandler, ICancelHandler
{
    public A_PartyManager partyManager;

    public List<RowHandler> rowHandlers;

    protected override void Start()
    {
        base.Start();
        if (partyManager == null)
        {
            partyManager = PlayerPartyHolder.Instance.partyManager;
        }
    }

    public void Recalculate()
    {
        if (this.rowHandlers == null)
        {
            this.rowHandlers = new List<RowHandler>();
        }
        RowHandler[] rowHandlers = gameObject.GetComponentsInChildren<RowHandler>();
        this.rowHandlers.Clear();
        foreach (RowHandler row in rowHandlers)
        {
            this.rowHandlers.Add(row);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.chosenTarget = this;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (rowHandlers == null || rowHandlers.Count == 0)
        {
            return;
        }
        Selected();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (rowHandlers == null || rowHandlers.Count == 0)
        {
            return;
        }
        Deselected();
    }

    public void Selected()
    {
        foreach (RowHandler row in rowHandlers)
        {
            row.Selected();
        }
    }

    public void Deselected()
    {
        foreach (RowHandler row in rowHandlers)
        {
            row.Deselected();
        }
    }

    public Selectable GetSelectableObject()
    {
        return this;
    }

    public List<ToolManager> GetTargets()
    {
        return partyManager.GetAll();
    }

    List<ToolManager> I_Targetable.GetTargets()
    {
        throw new System.NotImplementedException();
    }

    public void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }
}

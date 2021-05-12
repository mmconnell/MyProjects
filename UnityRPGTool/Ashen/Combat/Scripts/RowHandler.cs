using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Manager;
using System;

public class RowHandler : Selectable, I_Targetable, ISubmitHandler, ICancelHandler
{
    public PartyRow row;

    public A_PartyManager partyManager;

    public List<I_Targetable> targetables;

    protected override void Start()
    {
        base.Start();
        if (partyManager == null && PlayerPartyHolder.Instance != null)
        {
            partyManager = PlayerPartyHolder.Instance.partyManager;
        }
    }

    public void Recalculate()
    {
        if (this.targetables == null)
        {
            this.targetables = new List<I_Targetable>();
        }
        I_Targetable[] targetables = gameObject.GetComponentsInChildren<I_Targetable>();
        this.targetables.Clear();
        foreach (I_Targetable targetable in targetables)
        {
            if (!ReferenceEquals(this, targetable))
            {
                this.targetables.Add(targetable);
            }
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.chosenTarget = this;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (targetables == null || targetables.Count == 0)
        {
            return;
        }
        Selected();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (targetables == null || targetables.Count == 0)
        {
            return;
        }
        Deselected();
    }

    public void Selected()
    {
        foreach (I_Targetable targetable in targetables)
        {
            targetable.Selected();
        }
    }

    public void Deselected()
    {
        foreach (I_Targetable targetable in targetables)
        {
            targetable.Deselected();
        }
    }

    public Selectable GetSelectableObject()
    {
        return this;
    }

    public List<ToolManager> GetTargets()
    {
        return partyManager.GetRowTargets(row);
    }

    public void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }
}

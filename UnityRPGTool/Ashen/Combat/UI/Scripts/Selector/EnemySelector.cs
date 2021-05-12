using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Manager;
using System.Collections.Generic;

public class EnemySelector : A_CharacterSelector
{ 
    public EnemyArrow arrow;

    public GameObject attack;
    public Ability attackAbility;
    public RowHandler rowHandler;

    public override void RegisterToolManager(ToolManager toolManager)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        if (toolManager)
        {
        }
        rowHandler.Recalculate();
    }

    public override void UnregisterToolManager()
    {
        if (toolManager)
        {

        }
        this.toolManager = null;
        rowHandler.Recalculate();
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.chosenTarget = this;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        Selected();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        Deselected();
    }

    public override void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }

    public override void Selected()
    {
        if (arrow == null)
        {
            arrow = GetComponentInChildren<EnemyArrow>();
        }
        if (arrow != null)
        {
            arrow.selector.SetActive(true);
        }
    }

    public override void Deselected()
    {
        if (arrow == null)
        {
            arrow = GetComponentInChildren<EnemyArrow>();
        }
        if (arrow != null)
        {
            arrow.selector.SetActive(false);
        }
    }

    public override Selectable GetSelectableObject()
    {
        return this;
    }

    public override List<ToolManager> GetTargets()
    {
        List<ToolManager> toolManagers = new List<ToolManager>();
        toolManagers.Add(arrow.GetComponent<ToolManager>());
        return toolManagers;
    }
}

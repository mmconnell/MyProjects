using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;
using TMPro;
using JoshH.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PartyMemberManager : A_CharacterSelector, I_TriggerListener
{
    [HideInInspector]
    public PartyUIManager partyUIManager;

    public ArmorBarManager armorBarManager;
    public HealthBarManager healthBarManager;
    public ResourceBarManager resourceBarManager;

    public TextMeshProUGUI playerName;

    public RowHandler rowHandler;

    private UIGradient gradient;
    private ExtendedEffectTrigger primaryActionStart;
    private ExtendedEffectTrigger primaryActionEnd;
    private ExtendedEffectTrigger secondaryActionStart;
    private ExtendedEffectTrigger secondaryActionEnd;


    protected override void Awake()
    {
        base.Awake();
        gradient = GetComponent<UIGradient>();
        primaryActionStart = ExtendedEffectTriggers.GetEnum("PrimaryActionStart");
        primaryActionEnd = ExtendedEffectTriggers.GetEnum("PrimaryActionEnd");
        secondaryActionStart = ExtendedEffectTriggers.GetEnum("SecondaryActionStart");
        secondaryActionEnd = ExtendedEffectTriggers.GetEnum("SecondaryActionEnd");
    }

    public override void RegisterToolManager(ToolManager toolManager)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        if (toolManager)
        {
            playerName.text = toolManager.gameObject.name;
            this.healthBarManager.RegisterToolManager(toolManager);
            this.resourceBarManager.RegisterToolManager(toolManager);
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.RegisterTriggerListener(primaryActionStart, this);
            triggerTool.RegisterTriggerListener(primaryActionEnd, this);
            triggerTool.RegisterTriggerListener(secondaryActionStart, this);
            triggerTool.RegisterTriggerListener(secondaryActionEnd, this);
        }
        partyUIManager.Recalculate();
        rowHandler.Recalculate();
    }

    public override void UnregisterToolManager()
    {
        if (toolManager)
        {
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.UnregisterTriggerListener(primaryActionStart, this);
            triggerTool.UnregisterTriggerListener(primaryActionEnd, this);
            triggerTool.UnregisterTriggerListener(secondaryActionStart, this);
            triggerTool.UnregisterTriggerListener(secondaryActionEnd, this);
        }
        this.toolManager = null;
        playerName.text = "";
        healthBarManager.UnregisterToolManager();
        this.resourceBarManager.UnregisterToolManager();
        partyUIManager.Recalculate();
        rowHandler.Recalculate();
    }

    private ExtendedEffectTrigger triggerLock = null;

    public void OnTrigger(ExtendedEffectTrigger trigger)
    {
        if (trigger == primaryActionStart || trigger == secondaryActionStart)
        {
            if (triggerLock == null)
            {
                triggerLock = trigger;
                gradient.enabled = false;
            }
        }
        else if (trigger == primaryActionEnd || trigger == secondaryActionEnd)
        {
            if ((trigger == primaryActionEnd && triggerLock == primaryActionStart) || (trigger == secondaryActionEnd && triggerLock == secondaryActionStart))
            {
                gradient.enabled = true;
                triggerLock = null;
            }
        }
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

    public override void Selected()
    {
        //throw new System.NotImplementedException();
    }

    public override void Deselected()
    {
        //throw new System.NotImplementedException();
    }

    public override Selectable GetSelectableObject()
    {
        return this;
    }

    public override List<ToolManager> GetTargets()
    {
        List<ToolManager> toolManagers = new List<ToolManager>();
        toolManagers.Add(toolManager);
        return toolManagers;
    }

    public override void OnCancel(BaseEventData eventData)
    {
    }
}

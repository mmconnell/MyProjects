using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Manager;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Ashen.DeliverySystem;

public abstract class A_CharacterSelector : Selectable, I_Targetable, ISubmitHandler, ICancelHandler, IMoveHandler
{
    public ToolManager toolManager;

    protected ExtendedEffectTrigger primaryActionStart;
    protected ExtendedEffectTrigger primaryActionEnd;
    protected ExtendedEffectTrigger secondaryActionStart;
    protected ExtendedEffectTrigger secondaryActionEnd;

    protected override void Awake()
    {
        base.Awake();
        primaryActionStart = ExtendedEffectTriggers.GetEnum("PrimaryActionStart");
        primaryActionEnd = ExtendedEffectTriggers.GetEnum("PrimaryActionEnd");
        secondaryActionStart = ExtendedEffectTriggers.GetEnum("SecondaryActionStart");
        secondaryActionEnd = ExtendedEffectTriggers.GetEnum("SecondaryActionEnd");
    }

    public abstract void Deselected();

    public abstract Selectable GetSelectableObject();

    public abstract List<ToolManager> GetTargets();

    public abstract void OnCancel(BaseEventData eventData);

    public abstract void OnSubmit(BaseEventData eventData);

    public abstract void Selected();

    public abstract void SelectedSecondary();

    public abstract void RegisterToolManager(ToolManager toolManager);

    public abstract void UnregisterToolManager();

    public virtual void TurnSelectionStart() { }
    public virtual void TurnSelectionEnd() { }

    public virtual GameObject GetDisabler()
    {
        return gameObject;
    }

    public bool HasRegisteredToolManager()
    {
        return toolManager != null;
    }

    public ToolManager GetTarget()
    {
        return toolManager;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Manager;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Ashen.DeliverySystem;

public abstract class A_CharacterSelector : Selectable, I_Targetable, ISubmitHandler, ICancelHandler
{
    public ToolManager toolManager;

    public abstract void Deselected();

    public abstract Selectable GetSelectableObject();

    public abstract List<ToolManager> GetTargets();

    public abstract void OnCancel(BaseEventData eventData);

    public abstract void OnSubmit(BaseEventData eventData);

    public abstract void Selected();

    public abstract void RegisterToolManager(ToolManager toolManager);

    public abstract void UnregisterToolManager();

    public bool HasRegisteredToolManager()
    {
        return toolManager != null;
    }
}

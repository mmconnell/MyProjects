using UnityEngine;
using Manager;
using Ashen.DeliverySystem;
using TMPro;
using JoshH.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;

public class PartyMemberManager : A_CharacterSelector, I_TriggerListener, I_DamageListener, I_ThresholdListener
{
    [HideInInspector]
    public PartyUIManager partyUIManager;

    public ResourceBarManager healthBarManager;
    public ResourceBarManager resourceBarManager;

    public TextMeshProUGUI playerName;

    public UIGradient defaultColor;
    public UIGradient selectedColor;
    public UIGradient hitColor;
    public UIGradient targetedColor;

    public DOTweenAnimation buffAnimation;
    public DOTweenAnimation statDownAnimation;

    public StatusEffectSymbolManagerUI symbolManager;
    
    private Tween damageTakenTween;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void RegisterToolManager(ToolManager toolManager)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        if (toolManager)
        {
            playerName.text = toolManager.gameObject.name;
            this.healthBarManager.RegisterToolManager(toolManager, ResourceValues.Instance.health);
            ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
            this.resourceBarManager.RegisterToolManager(toolManager, resourceValueTool.AbilityResourceValue);
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.RegisterTriggerListener(primaryActionStart, this);
            triggerTool.RegisterTriggerListener(primaryActionEnd, this);
            triggerTool.RegisterTriggerListener(secondaryActionStart, this);
            triggerTool.RegisterTriggerListener(secondaryActionEnd, this);
            triggerTool.RegisterTriggerListener(ExtendedEffectTriggers.Instance.BuffRecieved, this);
            DamageTool damageTool = toolManager.Get<DamageTool>();
            damageTool.RegisterListener(DamageTypes.Instance.NORMAL, this);
            DOTweenAnimation[] anims = gameObject.GetComponents<DOTweenAnimation>();
            if (anims != null)
            {
                for (int x = 0; x < anims.Length; x++)
                {
                    if (anims[x].id == "Hit")
                    {
                        damageTakenTween = anims[x].tween;
                    }
                }
            }
            StatusTool statusTool = toolManager.Get<StatusTool>();
            statusTool.SymbolUI = symbolManager;
        }
        partyUIManager.Recalculate();
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
            triggerTool.UnregisterTriggerListener(ExtendedEffectTriggers.Instance.BuffRecieved, this);
            DamageTool damageTool = toolManager.Get<DamageTool>();
            damageTool.UnRegisterListener(DamageTypes.Instance.NORMAL, this);
            damageTakenTween = null;
            StatusTool statusTool = toolManager.Get<StatusTool>();
            statusTool.SymbolUI = null;
        }
        this.toolManager = null;
        playerName.text = "";
        healthBarManager.UnregisterToolManager();
        this.resourceBarManager.UnregisterToolManager();
        partyUIManager.Recalculate();
    }

    private ExtendedEffectTrigger triggerLock = null;

    public void OnTrigger(ExtendedEffectTrigger trigger)
    {
        if (trigger == primaryActionStart || trigger == secondaryActionStart)
        {
            if (triggerLock == null)
            {
                triggerLock = trigger;
                selectedColor.enabled = true;
            }
        }
        else if (trigger == primaryActionEnd || trigger == secondaryActionEnd)
        {
            if ((trigger == primaryActionEnd && triggerLock == primaryActionStart) || (trigger == secondaryActionEnd && triggerLock == secondaryActionStart))
            {
                selectedColor.enabled = false;
                triggerLock = null;
            }
        }
        else if (trigger == ExtendedEffectTriggers.Instance.BuffRecieved)
        {
            ListActionBundle bundles = new ListActionBundle();
            bundles.Bundles.Add(new DoTweenObjectProcessor()
            {
                tween = statDownAnimation.tween
            });
            ExecuteInputState.Instance.AddSupportingAction(bundles);
        }
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.chosenTarget = this;
    }

    public override void OnMove(AxisEventData eventData)
    {
        PlayerInputState.Instance.moveDirection = eventData.moveDir;
        base.OnMove(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        PlayerInputState.Instance.nextTarget = this;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
    }

    public override void Selected()
    {
        targetedColor.enabled = true;
    }

    public override void Deselected()
    {
        targetedColor.enabled = false;
    }

    public override void TurnSelectionStart()
    {
        selectedColor.enabled = true;
    }

    public override void TurnSelectionEnd()
    {
        selectedColor.enabled = false;
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
        PlayerInputState.Instance.backRequested = true;
    }

    public override GameObject GetDisabler()
    {
        return gameObject.transform.parent.gameObject;
    }

    public override void SelectedSecondary()
    {
        selectedColor.enabled = true;
    }

    public void OnDamageEvent(DamageEvent damageEvent)
    {
        if (damageEvent.damageAmount > 0)
        {
            ListActionBundle bundles = new ListActionBundle();

            bundles.Bundles.Add(new CombatLogProcessor()
            {
                message = toolManager.gameObject.name + " suffered " + damageEvent.damageAmount + " damage!",
            });
            bundles.Bundles.Add(new EnableTemporarilyProcessor()
            {
                toEnable = hitColor,
                totalTime = damageTakenTween.Duration(),
            });
            bundles.Bundles.Add(new DamageTextProcessor()
            {
                amount = damageEvent.damageAmount,
                location = gameObject.transform,
                damageTextPrefab = partyUIManager.damageTextPrefab,
                parent = partyUIManager.damageTextCanvas,
            });
            bundles.Bundles.Add(new DoTweenObjectProcessor()
            {
                tween = damageTakenTween,
                waitTime = .25f,
            });
            
            ExecuteInputState.Instance.AddSupportingAction(bundles);
        }
        if (damageEvent.damageAmount < 0)
        {
            ListActionBundle bundles = new ListActionBundle();

            bundles.Bundles.Add(new DamageTextProcessor()
            {
                amount = damageEvent.damageAmount,
                location = gameObject.transform,
                damageTextPrefab = partyUIManager.damageTextPrefab,
                parent = partyUIManager.damageTextCanvas,
            });
        }
    }

    public void OnThresholdEvent(ThresholdEventValue value)
    {
        ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
        if (value.resourceValue == resourceValueTool.AbilityResourceValue)
        {
            if (value.currentValue < value.previousValue)
            {
                ListActionBundle bundles = new ListActionBundle();

                bundles.Bundles.Add(new DamageTextProcessor()
                {
                    amount = value.previousValue - value.currentValue,
                    location = gameObject.transform,
                    damageTextPrefab = partyUIManager.damageTextPrefab,
                    parent = partyUIManager.damageTextCanvas,
                });
            }
        }
    }
}

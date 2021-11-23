using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Manager;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using DG.Tweening;

public class EnemySelector : A_CharacterSelector, I_TriggerListener, I_DamageListener
{ 
    private EnemyArrow arrow;
    private EnemyArrowSecondary arrowSecondary;
    private EnemyHealthCanvasHolder healthCanvasHolder;

    public GameObject attack;
    public AbilitySO attackAbility;

    private Tween turnStartTween;
    private Tween damageTakenTween;

    public override void RegisterToolManager(ToolManager toolManager)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        if (toolManager)
        {
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.RegisterTriggerListener(primaryActionStart, this);
            triggerTool.RegisterTriggerListener(primaryActionEnd, this);
            triggerTool.RegisterTriggerListener(secondaryActionStart, this);
            triggerTool.RegisterTriggerListener(secondaryActionEnd, this);
            DamageTool damageTool = toolManager.Get<DamageTool>();
            damageTool.RegisterListener(DamageTypes.Instance.NORMAL, this);
            DOTweenAnimation[] anims = toolManager.gameObject.GetComponents<DOTweenAnimation>();
            arrow = GetComponentInChildren<EnemyArrow>();
            arrowSecondary = GetComponentInChildren<EnemyArrowSecondary>();
            healthCanvasHolder = GetComponentInChildren<EnemyHealthCanvasHolder>();
            if (anims != null)
            {
                for (int x = 0; x < anims.Length; x++)
                {
                    if (anims[x].id == "ActionStart")
                    {
                        turnStartTween = anims[x].tween;
                    }
                    else if (anims[x].id == "DamageTaken")
                    {
                        damageTakenTween = anims[x].tween;
                    }
                }
            }
        }
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
            DamageTool damageTool = toolManager.Get<DamageTool>();
            damageTool.UnRegisterListener(DamageTypes.Instance.NORMAL, this);
            turnStartTween = null;
            damageTakenTween = null;
        }
        arrow = null;
        arrowSecondary = null;
        healthCanvasHolder = null;
        this.toolManager = null;
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

    public override void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }

    public override void Selected()
    {
        if (arrow != null)
        {
            arrow.selector.SetActive(true);
        }
        if (healthCanvasHolder != null)
        {
            healthCanvasHolder.canvas.gameObject.SetActive(true);
        }
    }

    public override void SelectedSecondary()
    {
        if (arrowSecondary != null)
        {
            arrowSecondary.selector.SetActive(true);
        }
        if (healthCanvasHolder != null)
        {
            healthCanvasHolder.canvas.gameObject.SetActive(true);
        }
    }

    public override void Deselected()
    {
        if (arrow != null)
        {
            arrow.selector.SetActive(false);
        }
        if (arrowSecondary != null)
        {
            arrowSecondary.selector.SetActive(false);
        }
        if (healthCanvasHolder != null)
        {
            healthCanvasHolder.canvas.gameObject.SetActive(false);
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

    public void OnTrigger(ExtendedEffectTrigger trigger)
    {
        if (trigger == primaryActionStart || trigger == secondaryActionStart)
        {
            ExecuteInputState.Instance.AddSupportingAction(new DoTweenObjectProcessor()
            {
                tween = turnStartTween,
                waitTime = 1f,
            });
        }
    }

    public void OnDamageEvent(DamageEvent damageEvent)
    {
        if (damageEvent.damageAmount > 0)
        {
            ListActionBundle listBundle = new ListActionBundle();
            listBundle.Bundles.Add(new CombatLogProcessor()
            {
                message = toolManager.gameObject.name + " suffered " + damageEvent.damageAmount + " damage!",
            });
            listBundle.Bundles.Add(new DoTweenObjectProcessor()
            {
                tween = damageTakenTween,
            });
            listBundle.Bundles.Add(new DamageTextProcessor()
            {
                amount = damageEvent.damageAmount,
                location = toolManager.gameObject.transform,
                damageTextPrefab = PoolManager.Instance.damageText,
            });
            
            ExecuteInputState.Instance.AddSupportingAction(listBundle);
            //PoolableDamageText dtPool = PoolManager.Instance.GetPoolManager(PoolManager.Instance.damageText).GetObject() as PoolableDamageText;
            //dtPool.transform.position = toolManager.transform.position;
            //ExecuteInputState.Instance.AddProcess(new DoTweenAnimationProcessor()
            //{
            //    tween = dtPool.mover,
            //});
            //ExecuteInputState.Instance.AddProcess(new DoTweenObjectProcessor()
            //{
            //    tween = dtPool.fader.tween,
            //});
        }
    }
}

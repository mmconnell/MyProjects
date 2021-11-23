using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Ashen.EquationSystem;
using Ashen.DeliverySystem;

public class ActionProcessor : I_CombatProcessor
{
    public ToolManager source;
    public float speed;
    public AbilitySpeedCategory speedCategory;
    public I_AbilityAction sourceAbility;
    private A_PartyManager sourceParty;
    private A_PartyManager targetParty;
    private I_TargetHolder targetHolder;

    private bool isInitial;
    private bool isValid;

    private TriggerTool triggerTool;

    public int id;

    public ActionProcessor(I_AbilityAction sourceAbility, ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_TargetHolder targetHolder)
    {
        this.sourceParty = sourceParty;
        this.targetParty = targetParty;
        this.sourceAbility = sourceAbility;
        this.source = source;
        DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
        if (sourceAbility.GetSpeedCategory() == null)
        {
            speedCategory = AbilitySpeedCategories.Instance.defaultSpeedCategory;
        }
        else
        {
            speedCategory = sourceAbility.GetSpeedCategory();
        }
        if (speedCategory.useSpeedCalculation)
        {
            speed = Random.Range(1f, 10f) * Mathf.Max(0.1f, DerivedAttributes.GetEnum("Speed").equation.Calculate(source.Get<DeliveryTool>(), packs.GetPack<EquationArgumentPack>()));
            if (sourceAbility.GetSpeedFactor() != null)
            {
                speed *= sourceAbility.GetSpeedFactor().Calculate(source.Get<DeliveryTool>(), packs.GetPack<EquationArgumentPack>());
            }
        }
        this.targetHolder = targetHolder;
        isInitial = true;
        isValid = true;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool HasAction()
    {
        return targetHolder.HasNextTarget(source, sourceParty, targetParty, sourceAbility);
    }

    public bool IsInitial()
    {
        return isInitial;
    }

    public I_CombatProcessor GetNextExecutable()
    {
        return targetHolder.ResolveTarget(source, sourceParty, targetParty, sourceAbility);
    }

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        if (isInitial)
        {
            isInitial = false;
            targetHolder.Initialize();
            triggerTool = source.Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.ActionStart);
            ResourceValueTool rvTool = source.Get<ResourceValueTool>();
            foreach (ResourceValue rv in ResourceValues.Instance)
            {
                int change = sourceAbility.GetResourceChange(rv, source);
                if (change < 0)
                {
                    rvTool.RemoveAmount(rv, -change);
                }
                else if (change > 0)
                {
                    rvTool.ApplyAmount(rv, change);
                }
            }
            if (sourceAbility.GetName() != null)
            {
                CombatLog.Instance.AddMessage(source.gameObject.name + " used " + sourceAbility.GetName() + "!");
                yield return new WaitForSeconds(.25f);
            }
            yield break;
        }

        if (HasNext(info))
        {
            ExecuteInputState.Instance.AddInturruptProcess(GetNextExecutable());
            yield break;
        }
        isValid = false;
        List<I_CombatProcessor> ongoingActions = ExecuteInputState.Instance.ongoingActions;
        for (int x = 0; x < ongoingActions.Count; x++)
        {
            yield return new WaitUntil(() => ongoingActions[x].IsFinished(info));
        }
        triggerTool.Trigger(ExtendedEffectTriggers.Instance.ActionEnd);
        ongoingActions.Clear();
    }

    public bool HasNext(CombatProcessorInfo info)
    {
        return targetHolder.HasNextTarget(source, sourceParty, targetParty, sourceAbility);
    }

    public bool IsValid(CombatProcessorInfo info)
    {
        return isValid;
    }

    public bool IsFinished(CombatProcessorInfo info)
    {
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Manager;

public class ExecuteInputState : SingletonScriptableObject<ExecuteInputState>, I_GameState
{
    private List<ActionProcessor> currentProcessor;
    private List<I_CombatProcessor> combatActions;
    private List<I_CombatProcessor> supportingActions;

    private List<ActionProcessor>[] primaryActionPerCategory;

    public List<I_CombatProcessor> ongoingActions;

    public ExtendedEffectTrigger actionStart;
    public ExtendedEffectTrigger actionEnd;

    public SubactionProcessor currentSubactionProcessor;

    public void Reset()
    {
        if (combatActions == null)
        {
            combatActions = new List<I_CombatProcessor>();
        }
        combatActions.Clear();
        if (primaryActionPerCategory == null)
        {
            primaryActionPerCategory = new List<ActionProcessor>[AbilitySpeedCategories.Count];
            foreach (AbilitySpeedCategory category in AbilitySpeedCategories.Instance)
            {
                primaryActionPerCategory[(int)category] = new List<ActionProcessor>();
            }
        }
        foreach (AbilitySpeedCategory category in AbilitySpeedCategories.Instance)
        {
            primaryActionPerCategory[(int)category].Clear();
        }
        if (supportingActions == null)
        {
            supportingActions = new List<I_CombatProcessor>();
        }
        supportingActions.Clear();
    }

    public void AddPrimaryAction(ActionProcessor actionHolder)
    {
        AbilitySpeedCategory speedCategory = actionHolder.sourceAbility.GetSpeedCategory();
        List<ActionProcessor> primaryActions = primaryActionPerCategory[(int)speedCategory];
        if (speedCategory.useSpeedCalculation)
        {
            for (int x = 0; x < primaryActions.Count; x++)
            {
                if (actionHolder.GetSpeed() > primaryActions[x].GetSpeed())
                {
                    primaryActions.Insert(x, actionHolder);
                    return;
                }
            }
        }
        primaryActions.Add(actionHolder);
    }

    public void AddInturruptAction(ActionProcessor actionProcessor)
    {
        currentProcessor.Insert(0, actionProcessor);
    }

    public void AddInturruptProcess(I_CombatProcessor processor)
    {
        combatActions.Insert(0, processor);
    }

    public void AddInturruptProcesses(List<I_CombatProcessor> processors)
    {
        combatActions.InsertRange(0, processors);
    }

    public void AddSupportingAction(I_CombatProcessor processor)
    {
        supportingActions.Add(processor);
    }

    public void ClearActions(ToolManager toolManager)
    {
        foreach (AbilitySpeedCategory category in AbilitySpeedCategories.Instance)
        {
            List<ActionProcessor> primaryActions = primaryActionPerCategory[(int)category];
            for (int x = 0; x < primaryActions.Count; x++)
            {
                if (primaryActions[x].source == toolManager)
                {
                    primaryActions.RemoveAt(x);
                    x--;
                }
            }
        }
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        CombatLog.Instance.ClearMessages();
        ongoingActions = new List<I_CombatProcessor>();
        CombatProcessorInfo info = new CombatProcessorInfo()
        {
            runner = request.runner,
        };
        foreach (AbilitySpeedCategory category in AbilitySpeedProcessOrder.Instance.speedCategoryOrder)
        {
            List<ActionProcessor> primaryActions = primaryActionPerCategory[(int)category];
            currentProcessor = primaryActions;
            while (primaryActions.Count > 0)
            {
                ActionProcessor actionProcessor = primaryActions[0];
                if (actionProcessor.IsValid(info))
                {
                    yield return actionProcessor.Execute(info);
                }
                else
                {
                    primaryActions.RemoveAt(0);
                    yield return null;
                }
                yield return ProcessCombatOptions(info);
            }
        }
        yield return null;
        this.Reset();
        response.nextState = EndRoundState.Instance;
    }

    public IEnumerator ProcessCombatOptions(CombatProcessorInfo info)
    {
        while (combatActions.Count > 0)
        {
            info.parentProcessorList = combatActions;
            I_CombatProcessor processor = combatActions[0];
            if (!processor.IsValid(info))
            {
                if (!processor.IsFinished(info))
                {
                    ongoingActions.Add(processor);
                }
                combatActions.RemoveAt(0);
            }
            else
            {
                yield return processor.Execute(info);
            }
            yield return ProcessSupportingActions(info);
        }
    }

    public IEnumerator ProcessSupportingActions(CombatProcessorInfo info)
    {
        while (supportingActions.Count > 0)
        {
            info.parentProcessorList = supportingActions;
            I_CombatProcessor supportingProcessor = supportingActions[0];
            if (!supportingProcessor.IsValid(info))
            {
                if (!supportingProcessor.IsFinished(info))
                {
                    ongoingActions.Add(supportingProcessor);
                }
                supportingActions.RemoveAt(0);
            }
            else
            {
                yield return supportingProcessor.Execute(info);
            }
        }
    }
}

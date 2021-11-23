using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;

public class SubactionProcessor : I_CombatProcessor
{
    public ActionExecutable actionExecutable;
    public AnimationExecutable animationExecutable;

    private bool isValid = true;
    
    private ToolManager lastTarget = null;

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        ExecuteInputState.Instance.currentSubactionProcessor = this;
        TriggerTool triggerTool;
        if (lastTarget != actionExecutable.target)
        {
            lastTarget = actionExecutable.target;
            triggerTool = actionExecutable.target.Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.Targeted);
            yield break;
        }
        if (animationExecutable != null)
        {
            yield return animationExecutable.Execute(info.runner);
        }
        yield return actionExecutable.Execute(info.runner);
        triggerTool = actionExecutable.target.Get<TriggerTool>();
        triggerTool.Trigger(ExtendedEffectTriggers.Instance.Effected);
        isValid = false;
    }

    public bool IsFinished(CombatProcessorInfo info)
    {
        if (!actionExecutable.IsFinished())
        {
            return false;
        }
        if (animationExecutable != null && !animationExecutable.IsFinished())
        {
            return false;
        }
        return true;
    }

    public bool IsValid(CombatProcessorInfo info)
    {
        return isValid;
    }
}

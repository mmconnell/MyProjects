using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Manager;

public class ExecuteInputState : SingletonScriptableObject<ExecuteInputState>, I_GameState
{
    private List<ActionHolder> combatActions;

    public ExtendedEffectTrigger actionStart;
    public ExtendedEffectTrigger actionEnd;

    public void Reset()
    {
        if (combatActions == null)
        {
            combatActions = new List<ActionHolder>();
        }
        else
        {
            combatActions.Clear();
        }
    }

    public void AddCombatAction(ActionHolder actionHolder)
    {
        for (int x = 0; x < combatActions.Count; x++)
        {
            if (actionHolder.GetSpeed() < combatActions[x].GetSpeed())
            {
                combatActions.Insert(x, actionHolder);
                return;
            }
        }
        combatActions.Add(actionHolder);
    }

    public void ClearActions(ToolManager toolManager)
    {
        for (int x = 0; x < combatActions.Count; x++)
        {
            if (combatActions[x].source == toolManager)
            {
                combatActions.RemoveAt(x);
                x--;
            }
        }
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        for (int x = 0; x < combatActions.Count; x++)
        {
            TriggerTool triggerTool = combatActions[x].source.Get<TriggerTool>();
            triggerTool.Trigger(actionStart);
            yield return request.runner.StartCoroutine(combatActions[x].Execute());
            yield return new WaitForSeconds(.5f);
            triggerTool.Trigger(actionEnd);
        }
        yield return null;
        this.Reset();
        response.nextState = PlayerInputState.Instance;
    }
}

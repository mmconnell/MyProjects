using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddPlayerAction : I_GameState
{
    private List<ActionProcessor> actionProcessors;

    public AddPlayerAction(List<ActionProcessor> actionHolder)
    {
        this.actionProcessors = actionHolder;
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerInputState inputState = PlayerInputState.Instance;
        ExecuteInputState executeInputState = ExecuteInputState.Instance;
        foreach (ActionProcessor processor in actionProcessors)
        {
            executeInputState.AddPrimaryAction(processor);
        }
        response.nextState = new MoveNextTurn();
        yield break;
    }
}

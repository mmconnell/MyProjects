using UnityEngine;
using System.Collections;

public class AddPlayerAction : I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerInputState inputState = PlayerInputState.Instance;
        ExecuteInputState executeInputState = ExecuteInputState.Instance;
        executeInputState.AddCombatAction(inputState.actionHolder);
        response.nextState = new MoveNextTurn();
        yield break;
    }
}

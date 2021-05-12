using UnityEngine;
using System.Collections;

public class MoveNextTurn : I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerInputState.Instance.backRequested = false;
        yield break;
    }
}

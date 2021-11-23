using UnityEngine;
using System.Collections;

public class TestStateLoader : SingletonScriptableObject<TestStateLoader>, I_GameState
{
    public I_GameState onStartState;
    public I_GameState onSelectState;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        while (true)
        {
            if (Input.GetKeyDown("joystick 1 button 6"))
            {
                GameStateManager selectState = CreateInstance<GameStateManager>();
                selectState.initialState = onSelectState;
                GameStateResponse newGameStateResponse = new GameStateResponse();
                yield return selectState.RunState(request, newGameStateResponse);
            }
            else if (Input.GetKeyDown("joystick 1 button 7"))
            {
                GameStateManager startState = CreateInstance<GameStateManager>();
                startState.initialState = onStartState;
                GameStateResponse newGameStateResponse = new GameStateResponse();
                yield return startState.RunState(request, newGameStateResponse);
            }
            yield return null;
        }
    }
}

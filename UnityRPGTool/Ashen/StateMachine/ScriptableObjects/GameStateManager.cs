using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SerializedScriptableObject, I_GameState
{
    public I_GameState initialState;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        if (initialState == null)
        {
            throw new System.Exception("Initial state is null and cannot be run. From: " + this.name);
        }
        if (!request.runner)
        {
            throw new System.Exception("Cannot initialize " + this.name + " because the passed in runner is null or not active");
        }
        I_GameState nextState = initialState;
        GameStateRequest newRequest = new GameStateRequest
        {
            lastState = this,
            runner = request.runner
        };
        while (nextState != null)
        {
            GameStateResponse newResponse = new GameStateResponse();
            Debug.Log(nextState);
            yield return request.runner.StartCoroutine(nextState.RunState(newRequest, newResponse));
            newRequest.lastState = nextState;
            nextState = newResponse.nextState;
        }
        response.completedState = this;
    }
}


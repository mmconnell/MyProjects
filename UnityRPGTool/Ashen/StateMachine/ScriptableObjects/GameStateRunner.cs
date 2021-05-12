using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class GameStateRunner : MonoBehaviour
{
    [Button]
    public void InitializeState(GameStateManager manager)
    {
        this.StartCoroutine(manager.RunState(
            new GameStateRequest
            {
                runner = this
            },
            new GameStateResponse()
       ));
    }
}

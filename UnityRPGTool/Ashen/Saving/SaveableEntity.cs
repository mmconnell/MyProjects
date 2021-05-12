using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

public class SaveableEntity : MonoBehaviour
{
    [SerializeField]
    private string id = string.Empty;

    public string Id
    {
        get
        {
            return id;
        }
    }

    [Button]
    private void GenerateNewId()
    {
        id = Guid.NewGuid().ToString();
    }

    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();

        foreach (I_Saveable savable in GetComponents<I_Saveable>())
        {
            state[savable.GetType().ToString()] = savable.CaptureState();
        }

        return state;
    }

    public void RestoreState(object stateObj)
    {
        Dictionary<string, object> state = (Dictionary<string, object>)stateObj;

        foreach (I_Saveable saveable in GetComponents<I_Saveable>())
        {
            string typeName = saveable.GetType().ToString();

            if (state.TryGetValue(typeName, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * This is an abstract class that is used to define a ScriptableObject that is a Singleton that allows 
 * for the retrieval of the ScriptableObject statically by name. This is mostly used for enumeratedLists.
 **/
public abstract class SingletonScriptableObject<T> : SerializedScriptableObject where T : SerializedScriptableObject
{
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

                if (!_instance)
                {
                    TimeManager.LoadAllScriptableObjects();
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                }
            }
            return _instance; 
        }
    }
}

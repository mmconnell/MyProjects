using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using System;

/**
 * The TimeManager is used to help the TimeRegister to be passed
 * Unity time updates
 **/
public class TimeManager : MonoBehaviour
{
    public TimeRegistry timeRegistry;
    public List<SerializedScriptableObject> preLoad;

    [ShowInInspector]
    public int Total
    {
        get
        {
            if (timeRegistry)
            {
                return TimeRegistry.GetTotal();
            }
            return 0;
        }
    }


    private void Awake()
    {
        timeRegistry.Clear();
        AddAllSingletonScriptableObjects();
    }

    private void Update()
    {
        timeRegistry.UpdateTime();
    }

    private void OnDestroy()
    {
        timeRegistry.Clear();
    }

    [Button]
    public void AddAllSingletonScriptableObjects()
    {
        List<SerializedScriptableObject> objects = StaticUtilities.FindAssetsByType<SerializedScriptableObject>();
        foreach (SerializedScriptableObject scriptableObject in objects)
        {
            Type type = scriptableObject.GetType();
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SingletonScriptableObject<>))
                {
                    if (preLoad == null)
                    {
                        preLoad = new List<SerializedScriptableObject>();
                    }
                    if (!preLoad.Contains(scriptableObject))
                    {
                        preLoad.Add(scriptableObject);
                    }
                    break;
                }
                type = type.BaseType;
            }
        }
    }

    public static void LoadAllScriptableObjects()
    {
        List<SerializedScriptableObject> objects = StaticUtilities.FindAssetsByType<SerializedScriptableObject>();
    }
}

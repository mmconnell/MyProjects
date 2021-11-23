using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PoolManager : SingletonScriptableObject<PoolManager>
{
    public DeliveryArgumentsPackPool deliveryArgumentsPool;
    public DeliveryContainerPool deliveryContainerPool;

    public GameObject damageText;

    public List<PrefabPool> defaultPrefabPools;

    [NonSerialized]
    private List<PrefabPool> allPrefabPools;

    public PrefabPool GetPoolManager(GameObject prefab, GameObject parent = null)
    {
        if (allPrefabPools == null)
        {
            allPrefabPools = new List<PrefabPool>();
            if (defaultPrefabPools != null)
            {
                allPrefabPools.AddRange(defaultPrefabPools);
            }
        }
        foreach (PrefabPool prefabPool in allPrefabPools)
        {
            if (prefabPool.prefab == prefab)
            {
                return prefabPool;
            }
        }
        PrefabPool newPool = CreateInstance<PrefabPool>();
        newPool.parent = parent;
        newPool.prefab = prefab;
        newPool.minPoolSize = 10;
        newPool.onMax = PoolMaxBehaviour.INCREASE_SIZE;
        allPrefabPools.Add(newPool);
        return newPool;
    }
}

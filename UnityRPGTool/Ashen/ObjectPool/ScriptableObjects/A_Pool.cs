using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

public abstract class A_Pool<T> : ScriptableObject where T : I_Poolable
{
    public int minPoolSize;
    public PoolMaxBehaviour onMax;
    [NonSerialized, ShowInInspector, HideInEditorMode]
    private Queue<T> pool;
    [NonSerialized, ShowInInspector, HideInEditorMode]
    private T[] activePool;
    [NonSerialized]
    private int activePoolIndex = 0;
    [NonSerialized, ShowInInspector, HideInEditorMode]
    private int currentSize;
    [NonSerialized]
    private bool initialized = false;

    public void Initialize()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;
        pool = new Queue<T>(minPoolSize);
        for (int x = 0; x < minPoolSize; x++)
        {
            pool.Enqueue(BuildObject());
        }
        activePool = new T[minPoolSize];
        currentSize = minPoolSize;
    }

    public T GetObject()
    {
        Initialize();
        T obj = default;
        if (pool.Count == 0)
        {
            int shiftAmount = 0;
            for (int x = 0; x < activePool.Length; x++)
            {
                if (!activePool[x].Enabled())
                {
                    pool.Enqueue(activePool[x]);
                    activePool[x] = default;
                    shiftAmount++;
                    activePoolIndex--;
                }
                else
                {
                    activePool[x - shiftAmount] = activePool[x];
                }
            }
            if (pool.Count == 0)
            {
                return HandleEmptyPool();
            }
        }
        obj = pool.Dequeue();
        activePool[activePoolIndex] = obj;
        activePoolIndex++;
        obj.Initialize();
        return obj;
    }

    private T HandleEmptyPool()
    {
        switch (onMax)
        {
            case PoolMaxBehaviour.DO_NOTHING:
                {
                    return default;
                }
            case PoolMaxBehaviour.RECYCLE:
                {
                    T obj = activePool[0];
                    for (int x = 0; x < activePoolIndex; x--)
                    {
                        activePool[x] = activePool[x + 1];
                    }
                    activePool[activePoolIndex - 1] = obj;
                    obj.Initialize();
                    return obj;
                }
            case PoolMaxBehaviour.INCREASE_SIZE:
                {
                    IncreasePoolSize();
                    T obj = pool.Dequeue();
                    activePool[activePoolIndex] = obj;
                    activePoolIndex++;
                    obj.Initialize();
                    return obj;
                }
        }
        return default;
    }

    private void IncreasePoolSize()
    {
        for (int x = 0; x < currentSize; x++)
        {
            pool.Enqueue(BuildObject());
        }
        currentSize *= 2;
        T[] newActiveArray = new T[currentSize];
        for (int x = 0; x < activePool.Length; x++)
        {
            newActiveArray[x] = activePool[x];
        }
        activePool = newActiveArray;
    }

    protected T BuildObject()
    {
        T obj = InternalBuildObject();
        obj.Disable();
        return obj;
    }

    protected abstract T InternalBuildObject();
}

public enum PoolMaxBehaviour
{
    INCREASE_SIZE, RECYCLE, DO_NOTHING
}
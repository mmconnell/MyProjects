using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/**
 * The EnumSO allows for the creation of ScriptableObjects as Enums
 **/
[Serializable]
public abstract class A_EnumSO<T, E> : SerializedScriptableObject where T : A_EnumSO<T, E> where E : A_EnumList<T, E>
{
    [OdinSerialize]
    protected int index;

    [NonSerialized]
    public bool destroy = false;

    [NonSerialized]
    private bool ensured = false;

    public int Index
    {
        get
        {
            if (!ensured)
            {
                EnsureEnumeration();
                ensured = true;
            }
            return index;
        }
        set
        {
            index = value;
        }
    }

    public List<T> GetListLocal()
    {
        return A_EnumList<T, E>.EnumList;
    }

    public static List<T> GetList()
    {
        return A_EnumList<T, E>.EnumList;
    }

   [Button]
    public void EnsureEnumeration()
    {
        if (A_EnumList<T, E>.Instance[index] != this)
        {
            A_EnumList<T, E>.Instance.Recount();
            if (A_EnumList<T, E>.Instance[index] != this)
            {
                A_EnumList<T, E>.Instance.Add((T)this);
            }
        }
    }

    public void OnDestroy()
    {
        destroy = true;
        A_EnumList<T, E>.Instance.Recount();
    }

    public void OnEnable()
    {
        EnsureEnumeration();
    }

    public override string ToString()
    {
        return name;
    }

    public static explicit operator int(A_EnumSO<T, E> v)
    {
        if (ReferenceEquals(v, null))
        {
            return -1;
        }
        return v.Index;
    }
}

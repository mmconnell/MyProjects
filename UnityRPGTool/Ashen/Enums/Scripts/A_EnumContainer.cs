using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;

/**
 * The EnumContainer allows for grouping certian EnumSOs together
 **/
[Serializable]
public abstract class A_EnumContainer<T, E> : ISerializable where T : A_EnumSO<T, E> where E : A_EnumList<T, E>
{
    [AutoPopulate, EnumSODropdown]
    public List<T> enums;

    public A_EnumContainer(SerializationInfo info, StreamingContext context)
    {
        List<int> enumNums = (List<int>)info.GetValue(nameof(enums), typeof(List<int>));
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        List<int> enumNums = new List<int>();
        foreach (T e in enums)
        {
            enumNums.Add((int)e);
        }
        info.AddValue(nameof(enums), enumNums);
    }
}
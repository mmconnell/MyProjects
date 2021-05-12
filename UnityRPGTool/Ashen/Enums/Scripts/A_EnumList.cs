using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * The EnumList holds all the Enums of a specifict type. It also will enumerate all the EnumSO's in the list.
 **/
public abstract class A_EnumList<T,E> : SingletonScriptableObject<E>, IEnumerable<T> where T : A_EnumSO<T, E> where E : A_EnumList<T, E>
{
    [OdinSerialize, AutoPopulate]
    private List<T> enumList = default;

    [NonSerialized]
    private Dictionary<string, T> enumMap;
    private Dictionary<string, T> EnumMap
    {
        get
        {
            if (enumMap == null)
            {
                enumMap = new Dictionary<string, T>();
                foreach (T enumSo in enumList)
                {
                    enumMap.Add(enumSo.name, enumSo);
                }
            }
            return enumMap;
        }
    }

    public static T GetEnum(string enumName)
    {
        if (Instance.EnumMap.TryGetValue(enumName, out T enumSo))
        {
            return enumSo;
        }
        throw new Exception("Could not find enum: " + enumName + " from list: " + Instance.name);
    }

    public static List<T> EnumList
    {
        get
        {
            return Instance.enumList;
        }
    }

    public static int Count
    {
        get
        {
            if (Instance.enumList == null)
            {
                Logger.ErrorLog("enumList in EnumList cannot be null");
                return 0;
            }
            return Instance.enumList.Count;
        }
    }

    public T this[int i]
    {
        get
        {
            if (enumList == null || i < 0 || i >= enumList.Count)
            {
                return null;
            }
            return enumList[i];
        }
    }

    private void OnEnable()
    {
        Recount();
    }

    [Button]
    public void Recount()
    {
        if (enumList != null)
        {
            for (int x = 0; x < enumList.Count; x++)
            {
                if (enumList[x] == null || enumList[x].destroy)
                {
                    enumList.RemoveAt(x);
                    x--;
                }
                else
                {
                    enumList[x].Index = x;
                }
                
            }
        }
    }

    public void Add(T t)
    {
        enumList.Add(t);
        Recount();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int x = 0; x < enumList.Count; x++)
        {
            yield return enumList[x];
        }
    }
}
 
using UnityEngine;
using System.Collections;

public abstract class A_DeliveryArgumentPack<T> : I_DeliveryArgumentPack where T : A_DeliveryArgumentPack<T>
{
    private static int index = -1;
    public static int Index
    {
        get
        {
            return index;
        }
    }
    public abstract void Clear();

    public abstract I_DeliveryArgumentPack Initialize();

    public void SetIndex(int index)
    {
        A_DeliveryArgumentPack<T>.index = index;
    }

    public int GetIndex()
    {
        return Index;
    }
}

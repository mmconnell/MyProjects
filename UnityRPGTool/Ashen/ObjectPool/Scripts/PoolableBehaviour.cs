using UnityEngine;
using System.Collections;

public class PoolableBehaviour : MonoBehaviour, I_Poolable
{
    public void Disable()
    {
        enabled = false;
    }

    public bool Enabled()
    {
        return enabled;
    }

    public void Initialize()
    {
        enabled = true;
    }
}

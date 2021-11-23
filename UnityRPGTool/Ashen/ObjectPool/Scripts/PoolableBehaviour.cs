using UnityEngine;
using System.Collections;

public class PoolableBehaviour : MonoBehaviour, I_Poolable
{
    public virtual void Disable()
    {
        enabled = false;
    }

    public virtual bool Enabled()
    {
        return enabled;
    }

    public virtual void Initialize()
    {
        enabled = true;
    }
}

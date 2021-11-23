using UnityEngine;
using System.Collections;

public class PoolableWrapper : PoolableBehaviour
{
    public PoolableBehaviour subBehaviour;

    public override void Disable()
    {
        subBehaviour.Disable();
    }

    public override bool Enabled()
    {
        return subBehaviour.Enabled();
    }

    public override void Initialize()
    {
        subBehaviour.Initialize();
    }
}

using UnityEngine;
using System.Collections;

public class DeliveryArgumentsPackPool : A_Pool<DeliveryArgumentPacks>
{
    protected override DeliveryArgumentPacks InternalBuildObject()
    {
        return new DeliveryArgumentPacks();
    }
}

using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

public class DeliveryContainerPool : A_Pool<DeliveryContainer>
{
    protected override DeliveryContainer InternalBuildObject()
    {
        return new DeliveryContainer();
    }
}

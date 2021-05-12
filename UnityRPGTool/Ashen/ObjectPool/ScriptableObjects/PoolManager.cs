using UnityEngine;
using System.Collections;

public class PoolManager : SingletonScriptableObject<PoolManager>
{
    public DeliveryArgumentsPackPool deliveryArgumentsPool;
    public DeliveryContainerPool deliveryContainerPool;
}

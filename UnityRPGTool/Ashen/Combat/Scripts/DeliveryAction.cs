using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Manager;

public class DeliveryAction : I_CombatAction
{
    public DeliveryPackBuilder deliveryPackBuilder;
    public ToolManager target;
    public ToolManager source;
    public GameObject animation;

    public void ExecuteAction()
    {
        if (animation != null)
        {
            Object.Instantiate(animation, target.GetComponent<AnimationCenterTracker>().animationCenter.transform);
        }
        DeliveryArgumentPacks pack = PoolManager.Instance.deliveryArgumentsPool.GetObject();
        DeliveryContainer container = PoolManager.Instance.deliveryContainerPool.GetObject();
        container.AddPrimaryEffect(deliveryPackBuilder.deliveryPack.Build(source.Get<DeliveryTool>(), target.Get<DeliveryTool>(), pack));
        DeliveryUtility.Deliver(container, source.Get<DeliveryTool>(), target.Get<DeliveryTool>(), pack);
        pack.Disable();
        container.Disable();
    }

    public float GetSpeedFactor()
    {
        return 1f;
    }

    public bool ResolveSelection()
    {
        throw new System.NotImplementedException();
    }
}

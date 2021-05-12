using UnityEngine;
using UnityEditor;
using Ashen.DeliverySystem;
using Manager;

public class SingleTargetAbility : A_Ability
{
    private Target singleTarget;

    public DeliveryPackBuilder deliveryPack;

    public override Target GetTargetType()
    {
        if (singleTarget == null)
        {
            singleTarget = Targets.GetEnum("SingleTarget");
        }
        return singleTarget;
    }

    public override void ResolveTarget(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        DeliveryArgumentPacks pack = PoolManager.Instance.deliveryArgumentsPool.GetObject();
        DeliveryContainer container = PoolManager.Instance.deliveryContainerPool.GetObject();
        container.AddPrimaryEffect(deliveryPack.deliveryPack.Build(actionHolder.source.Get<DeliveryTool>(), actionHolder.targetHodler.GetTargets()[0].Get<DeliveryTool>(), pack));
        DeliveryUtility.Deliver(container, actionHolder.source.Get<DeliveryTool>(), actionHolder.targetHodler.GetTargets()[0].Get<DeliveryTool>(), pack);
        pack.Disable();
        container.Disable();
    }
}
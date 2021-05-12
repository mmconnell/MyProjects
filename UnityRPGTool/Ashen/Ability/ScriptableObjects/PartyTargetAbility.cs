using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Manager;

public class PartyTargetAbility : A_Ability
{
    private Target partyTarget;

    public DeliveryPackBuilder deliveryPack;

    public override Target GetTargetType()
    {
        if (partyTarget == null)
        {
            partyTarget = Targets.GetEnum("PartyTarget");
        }
        return partyTarget;
    }

    public override void ResolveTarget(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        foreach (ToolManager manager in actionHolder.targetHodler.GetTargets())
        {
            DeliveryArgumentPacks pack = PoolManager.Instance.deliveryArgumentsPool.GetObject();
            DeliveryContainer container = PoolManager.Instance.deliveryContainerPool.GetObject();
            container.AddPrimaryEffect(deliveryPack.deliveryPack.Build(actionHolder.source.Get<DeliveryTool>(), manager.Get<DeliveryTool>(), pack));
            DeliveryUtility.Deliver(container, actionHolder.source.Get<DeliveryTool>(), manager.Get<DeliveryTool>(), pack);
            pack.Disable();
            container.Disable();
        }
    }
}

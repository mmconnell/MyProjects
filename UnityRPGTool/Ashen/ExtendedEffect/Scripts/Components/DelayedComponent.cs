using Ashen.DeliverySystem;
using Manager;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    [Serializable]
    public class DelayedComponent : A_SimpleComponent
    {
        public I_Effect effect;

        public DelayedComponent() { }
        public DelayedComponent(I_Effect effect)
        {
            this.effect = effect;
        }

        public override void End(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
            DeliveryContainer deliveryContainer = PoolManager.Instance.deliveryContainerPool.GetObject();
            deliveryContainer.AddPrimaryEffect(effect);
            DeliveryUtility.Deliver(deliveryContainer, dse.owner, dse.target, packs);
            packs.Disable();
            deliveryContainer.Disable();
        }
    }
}
using UnityEngine;
using System.Collections;
using Manager;

namespace Ashen.DeliverySystem
{
    public class ApplyEffect : I_TagOperation
    {
        public I_EffectBuilder effect;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryContainer container = PoolManager.Instance.deliveryContainerPool.GetObject();
            container.AddPrimaryEffect(this.effect.Build(owner, target, deliveryArguments));
            DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
            DeliveryUtility.Deliver(container, owner, target, packs);
            packs.Disable();
            container.Disable();
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            return visualization + "Apply " + effect.ToString();
        }
    }
}
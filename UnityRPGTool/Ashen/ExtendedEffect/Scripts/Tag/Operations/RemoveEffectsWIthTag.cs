using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using System.Collections.Generic;
using Manager;

namespace Ashen.DeliverySystem
{
    public class RemoveEffectsWIthTag : I_TagOperation
    {
        [OdinSerialize]
        public ExtendedEffectTag statusEffectTag;


        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool tDeliveryTool = target as DeliveryTool;
            if (tDeliveryTool)
            {
                StatusTool tStatusTool = tDeliveryTool.toolManager.Get<StatusTool>();
                if (tStatusTool)
                {
                    tStatusTool.DisableAllStatusEffectsWithTag(statusEffectTag);
                }
            }
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            return visualization + "Disable StatusEffects with tag: " + statusEffectTag.ToString();
        }
    }
}
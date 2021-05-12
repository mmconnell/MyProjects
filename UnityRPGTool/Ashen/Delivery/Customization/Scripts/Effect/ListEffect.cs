using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ListEffect : I_Effect
    {
        public List<I_Effect> effects;

        public ListEffect()
        {
            effects = new List<I_Effect>();
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            for (int x = 0; x < effects.Count; x++)
            {
                effects[x].Apply(owner, target, targetDeliveryResult, deliveryArguments);
            }
        }
    }
}
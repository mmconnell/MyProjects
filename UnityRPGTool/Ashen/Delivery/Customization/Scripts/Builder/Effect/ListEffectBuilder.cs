using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class ListEffectBuilder : I_EffectBuilder
    {
        [ListDrawerSettings(Expanded = true)]
        public List<I_EffectBuilder> effects;

        public ListEffectBuilder()
        {
            effects = new List<I_EffectBuilder>();
        }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ListEffect listEffect = new ListEffect();
            List<I_Effect> newEffects = listEffect.effects;
            foreach (I_EffectBuilder effect in effects)
            {
                newEffects.Add(effect.Build(owner, target, deliveryArguments));
            }
            return listEffect;
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < effects.Count; x++)
            {
                vis += effects[x].visualize(depth);
                if (x != effects.Count - 1)
                {
                    vis += "\n";
                }
            }
            return vis;
        }
    }
}
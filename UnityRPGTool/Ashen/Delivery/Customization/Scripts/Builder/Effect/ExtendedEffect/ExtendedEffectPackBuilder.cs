using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    public class ExtendedEffectPackBuilder : I_EffectBuilder
    {
        [OdinSerialize]
        [HorizontalGroup("StatusEffect")]
        [LabelWidth(50)]
        public StatusEffectScriptableObject Copy;
        [OdinSerialize]
        public I_TickerPack TickerPack;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new StatusEffectPack(Copy, TickerPack != null ? TickerPack.Build(owner, target, deliveryArguments.GetPack<EquationArgumentPack>()) : null);
        }

        public override string ToString()
        {
            return Copy.ToString();
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Apply " + Copy.name;
            return vis;
        }
    }
}
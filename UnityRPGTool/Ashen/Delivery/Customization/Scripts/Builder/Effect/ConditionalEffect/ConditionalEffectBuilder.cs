using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class ConditionalEffectBuilder : I_EffectBuilder
    {
        public I_EffectCondition effectCondition;
        public I_EffectBuilder effectResult;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            if (effectCondition.Check(owner, target, deliveryArguments))
            {
                return effectResult.Build(owner, target, deliveryArguments);
            }
            return null;
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "if(" + effectCondition.visualize() + ")\n";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "{\n" + effectResult.visualize(depth + 1) + "\n";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "}";
            return visualization;
        }
    }
}
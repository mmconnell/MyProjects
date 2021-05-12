using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class MultiTagOperation : I_TagOperation
    {
        [AutoPopulate, InlineProperty, ListDrawerSettings(Expanded = true)]
        public List<I_TagOperation> operations;
        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            foreach (I_TagOperation operation in operations)
            {
                if (tagState.continueOperation)
                {
                    operation.Operate(owner, target, tagState, deliveryArguments);
                }
            }
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < operations.Count; x++)
            {
                visualization += operations[x].visualize(depth);
                if (x != operations.Count - 1)
                {
                    visualization += "\n";
                }
            }
            return visualization;
        }
    }
}
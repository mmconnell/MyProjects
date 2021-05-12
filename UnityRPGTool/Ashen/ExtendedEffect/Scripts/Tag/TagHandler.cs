using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class TagHandler
    {
        [ReadOnly, HideLabel, FoldoutGroup("visualization")]
        [InfoBox("$visualize")]
        public string visualize;

        [AutoPopulate, InlineProperty]
        public List<I_TagOperation> tagOperations;

        public TagState Operate(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            TagState state = new TagState
            {
                continueOperation = true,
                validStatusEffect = true,
                appliedTags = new List<ExtendedEffectTag>(),
            };
            foreach (I_TagOperation tagOperation in tagOperations)
            {
                if (!state.continueOperation)
                {
                    break;
                }
                tagOperation.Operate(owner, target, state, deliveryArguments);
            }
            return state;
        }

        [Button]
        public void visualizeHandler()
        {
            visualize = "";
            foreach (I_TagOperation operation in tagOperations)
            {
                visualize += operation.visualize(0) + "\n";
            }
        }
    }
}
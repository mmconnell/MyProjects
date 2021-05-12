using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class DeliveryPackBuilder
    {
        [ReadOnly, HideLabel, FoldoutGroup("visualization")]
        [InfoBox("$visualize")]
        public string visualize;

        [HideLabel, FoldoutGroup("Effect"), InlineProperty, Indent]
        public I_EffectBuilder deliveryPack;

        [Button]
        public void visualizeHandler()
        {
            visualize = deliveryPack.visualize(0);
        }
    }
}
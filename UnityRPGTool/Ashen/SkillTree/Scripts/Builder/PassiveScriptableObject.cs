using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Ashen.DeliverySystem;

namespace Ashen.SkillTree
{
    public class PassiveScriptableObject : SerializedScriptableObject
    {
        [OdinSerialize, Hide]
        public SkillNodeEffectBuilder statusEffect;

        public I_ExtendedEffect Clone(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks)
        {
            return statusEffect.Build(owner, target, deliveryArgumentPacks);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
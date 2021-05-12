using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Ashen.DeliverySystem;

namespace Ashen.SkillTree
{
    public class SkillNodeEffectBuilder
    {
        [OdinSerialize, HideLabel, BoxGroup("Key")]
        public string key;

        [ListDrawerSettings(Expanded = true), InlineProperty, AutoPopulate]
        public List<I_ComponentBuilder> baseStatusEffects;

        public I_ExtendedEffect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks)
        {
            return new ExtendedEffect(baseStatusEffects, null, key, owner, target, deliveryArgumentPacks); ;
        }
    }
}
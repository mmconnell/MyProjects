using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ListEffect : I_Effect, ISerializable
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

        public ListEffect(SerializationInfo info, StreamingContext context)
        {
            int length = info.GetInt32(nameof(effects) + "-Count");
            effects = new List<I_Effect>();
            for (int x = 0; x < length; x++)
            {
                string effectsName = nameof(effects) + "-" + x;
                Type effectType = Type.GetType(info.GetString(effectsName + "-Type"));
                effects.Add((I_Effect)info.GetValue(effectsName, effectType));
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(effects) + "-Count", effects.Count);
            for (int x = 0; x < effects.Count; x++)
            {
                string effectsName = nameof(effects) + "-" + x;
                info.AddValue(effectsName, effects[x]);
                info.AddValue(effectsName + "-Type", effects[x].GetType().FullName);
            }
        }
    }
}
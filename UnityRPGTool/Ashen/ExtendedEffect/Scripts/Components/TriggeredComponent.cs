using Ashen.DeliverySystem;
using Manager;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    [Serializable]
    public class TriggeredComponent : A_SimpleComponent, ISerializable
    {
        [OdinSerialize]
        private I_Effect effect;
        [OdinSerialize]
        private ExtendedEffectTrigger[] triggers;

        public TriggeredComponent() { }

        public TriggeredComponent(I_Effect effect, ExtendedEffectTrigger[] triggers)
        {
            this.effect = effect;
            this.triggers = triggers;
        }

        public override void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container)
        {
            for (int x = 0; x < triggers.Length; x++)
            {
                if (statusTrigger == triggers[x])
                {
                    dse.deliveryContainer.AddPrimaryEffect(effect);
                    break;
                }
            }
        }

        public override ExtendedEffectTrigger[] GetStatusTriggers()
        {
            return triggers;
        }

        public TriggeredComponent(SerializationInfo info, StreamingContext context)
        {
            Type effectType = Type.GetType(info.GetString(nameof(effect) + "-Type"));
            effect = (I_Effect)info.GetValue(nameof(effect), effectType);
            int length = info.GetInt32(nameof(triggers) + "-Count");
            triggers = new ExtendedEffectTrigger[length];
            for (int x = 0; x < length; x++)
            {
                triggers[x] = ExtendedEffectTriggers.Instance[info.GetInt32(nameof(triggers) + "-" + x)];
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(effect), effect);
            info.AddValue(nameof(effect) + "-Type", effect.GetType().FullName);
            info.AddValue(nameof(triggers) + "-Count", triggers.Length);
            for (int x = 0; x < triggers.Length; x++)
            {
                info.AddValue(nameof(triggers) + "-" + x, (int)triggers[x]);
            }
        }
    }
}
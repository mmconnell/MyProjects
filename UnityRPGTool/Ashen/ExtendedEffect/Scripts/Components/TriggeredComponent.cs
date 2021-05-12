using Ashen.DeliverySystem;
using Manager;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    public class TriggeredComponent : A_SimpleComponent
    {
        //[OdinSerialize]
        //public Character_Trigger_Enum CharacterTrigger;
        [OdinSerialize]
        public I_Effect effect;
        private new ExtendedEffectTrigger[] statusTriggers = new ExtendedEffectTrigger[5];

        //public TriggeredStatusEffect(Character_Trigger_Enum statusTrigger, I_DeliveryPack deliveryPack, params StatusTrigger[] statusTriggers)
        //{
        //    CharacterTrigger = statusTrigger;
        //   DeliveryPack = deliveryPack;
        //    this.statusTriggers = statusTriggers;
        //}

        public override void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container)
        {
            for (int x = 0; x < statusTriggers.Length; x++)
            {
                if (statusTrigger == statusTriggers[x])
                {
                    dse.deliveryContainer.AddPrimaryEffect(effect);
                    break;
                }
            }
        }

        public override ExtendedEffectTrigger[] GetStatusTriggers()
        {
            return statusTriggers;
        }
    }
}
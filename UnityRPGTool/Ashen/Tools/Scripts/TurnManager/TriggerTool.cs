using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Manager
{
    public class TriggerTool : A_EnumeratedTool<TriggerTool>
    {
        private List<I_TriggerListener>[] triggerListeners;

        public override void Initialize()
        {
            base.Initialize();
            triggerListeners = new List<I_TriggerListener>[ExtendedEffectTriggers.Count];
            for (int x = 0; x < triggerListeners.Length; x++)
            {
                triggerListeners[x] = new List<I_TriggerListener>();
            }
        }

        public void Trigger(ExtendedEffectTrigger trigger)
        {
           foreach (I_TriggerListener listener in triggerListeners[(int)trigger])
            {
                listener.OnTrigger(trigger);
            }
        }

        public void RegisterTriggerListener(ExtendedEffectTrigger trigger, I_TriggerListener listener)
        {
            triggerListeners[(int)trigger].Add(listener);
        }

        public void UnregisterTriggerListener(ExtendedEffectTrigger trigger, I_TriggerListener listener)
        {
            triggerListeners[(int)trigger].Remove(listener);
        }
    }
}
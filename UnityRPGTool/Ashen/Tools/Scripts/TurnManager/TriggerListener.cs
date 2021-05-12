using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

namespace Manager
{
    public interface I_TriggerListener
    {
        void OnTrigger(ExtendedEffectTrigger trigger);
    }
}
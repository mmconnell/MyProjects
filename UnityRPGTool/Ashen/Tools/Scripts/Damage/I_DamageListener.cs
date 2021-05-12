using UnityEngine;
using System.Collections;

namespace Manager
{
    public interface I_DamageListener
    {
        void OnDamageEvent(DamageEvent damageEvent);
    }
}
using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using System;
using System.Collections.Generic;

namespace Manager
{
    /**
     * This MonoBehaviour will manage applying damage to a character
     **/
    public class DamageTool : A_EnumeratedTool<DamageTool>
    {
        ResistanceTool resistanceTool;

        private List<I_DamageListener>[] damageTypeListeners;
        private List<I_DamageListener> listeners;

        public override void Initialize()
        {
            base.Initialize();
            listeners = new List<I_DamageListener>();
            damageTypeListeners = new List<I_DamageListener>[DamageTypes.Count];
            for (int x = 0; x < DamageTypes.Count; x++)
            {
                damageTypeListeners[x] = new List<I_DamageListener>();
            }
        }

        private void Start()
        {
            resistanceTool = toolManager.Get<ResistanceTool>();
        }

        public void RegisterListener(DamageType damageType, I_DamageListener listener)
        {
            damageTypeListeners[(int)damageType].Add(listener);
        }

        public void RegisterListener(I_DamageListener listener)
        {
            listeners.Add(listener);
        }

        public void UnRegisterListener(DamageType damageType, I_DamageListener listener)
        {
            damageTypeListeners[(int)damageType].Remove(listener);
        }

        public void UnRegisterListener(I_DamageListener listener)
        {
            listeners.Remove(listener);
        }

        public void Report(DamageType damageType, int amount)
        {
            int damageNum = (int)damageType;
            DamageEvent damageEvent = new DamageEvent
            {
                damageType = damageType,
                damageAmount = amount
            };
            for (int x = 0; x < damageTypeListeners[damageNum].Count; x++)
            {
                I_DamageListener listener = damageTypeListeners[damageNum][x];
                listener.OnDamageEvent(damageEvent);
            }
            for (int x = 0; x < listeners.Count; x++)
            {
                I_DamageListener listener = listeners[x];
                listener.OnDamageEvent(damageEvent);
            }
        }

        public void TakeDamage(DamageType damageType, int damage)
        {
            Report(damageType, damage);
        }

        public int GetDamage(DamageType damageType, int damage, DeliveryArgumentPacks deliveryArguments)
        {
            float resistanceMult = 1;
            resistanceMult = resistanceTool.GetResistancePercentage(damageType, deliveryArguments.GetPack<EquationArgumentPack>());
            int finalDamage = (int)Math.Round(damage * resistanceMult, 0);
            return finalDamage;
        }
    }
}

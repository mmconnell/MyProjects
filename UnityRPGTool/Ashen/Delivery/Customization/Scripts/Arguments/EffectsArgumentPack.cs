using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class EffectsArgumentPack : A_DeliveryArgumentPack<EffectsArgumentPack>
    {
        private static readonly List<I_Effect> EMPTY_EFFECTS = new List<I_Effect>();
        private List<I_Effect> effects;

        public DamageType WeaponDamageType { get; set; }

        public EffectsArgumentPack()
        {
            effects = new List<I_Effect>();
        }

        public override I_DeliveryArgumentPack Initialize()
        {
            return new EffectsArgumentPack();
        }

        public List<I_Effect> GetEffects()
        {
            if (effects == null)
            {
                return EMPTY_EFFECTS;
            }
            return effects;
        }

        public void AddEffect(I_Effect effect)
        {
            if (effects == null)
            {
                effects = new List<I_Effect>();
            }
            effects.Add(effect);
        }

        public void AddEffects(List<I_Effect> effects)
        {
            if (this.effects == null)
            {
                this.effects = new List<I_Effect>();
            }
            this.effects.AddRange(effects);
        }

        public override void Clear()
        {
            if (effects != null)
            {
                effects.Clear();
            }
        }
    }
}
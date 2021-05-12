using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Manager
{
    public class AbilityHolder : A_EnumeratedTool<AbilityHolder>
    {
        // Typically only used for enemies
        public A_Ability defaultAbilities;

        private List<A_Ability> abilities;

        void Start()
        {
            abilities = new List<A_Ability>();
        }

        public List<A_Ability> GetAbilities()
        {
            List<A_Ability> abilities = new List<A_Ability>(this.abilities);
            return abilities;
        }

        public bool GrantAbility(A_Ability ability)
        {
            if (!abilities.Contains(ability))
            {
                abilities.Add(ability);
                return true;
            }
            return false;
        }

        public void RevokeAbility(A_Ability ability)
        {
            abilities.Remove(ability);
        }

        public A_Ability GetRandomAbility()
        {
            int random = Random.Range(0, abilities.Count);
            return abilities[random];
        }
    }
}
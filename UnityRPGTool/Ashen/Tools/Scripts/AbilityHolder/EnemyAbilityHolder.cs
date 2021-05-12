using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Manager
{
    public class EnemyAbilityHolder : A_EnumeratedTool<EnemyAbilityHolder>
    {
        public List<A_Ability> abilities;

        public A_Ability GetRandomAbility()
        {
            int random = Random.Range(0, abilities.Count);
            return abilities[random];
        }
    }
}
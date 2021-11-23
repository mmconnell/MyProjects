using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;

namespace Manager
{
    public class AbilityHolder : A_EnumeratedTool<AbilityHolder>
    {
        private Ability attackAbility;
        public Ability AttackAbility { get { return attackAbility; } }

        private Ability defendAbility;
        public Ability DefendAbility { get { return defendAbility; } }

        [ShowInInspector]
        private List<Ability> abilities;

        private Dictionary<string, Ability> identifierToAbility;

        [OdinSerialize]
        private AbilityHolderConfiguration abilityHolderConfiguration = default;
        private AbilityHolderConfiguration AbilityHolderConfiguration
        {
            get
            {
                if (abilityHolderConfiguration == null)
                {
                    return DefaultValues.Instance.defaultAbilityHolderConfiguration;
                }
                return abilityHolderConfiguration;
            }
        }

        public void Initialize(AbilityHolderConfiguration config)
        {
            abilityHolderConfiguration = config;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            abilities = new List<Ability>();
            identifierToAbility = new Dictionary<string, Ability>();
            foreach (AbilitySO abilitySO in AbilityHolderConfiguration.DefaultAbilities)
            {
                abilities.Add(abilitySO.abilityBuilder.BuildAbility());
            }
            attackAbility = AbilityHolderConfiguration.AttackAbility.abilityBuilder.BuildAbility();
            defendAbility = AbilityHolderConfiguration.DefendAbility.abilityBuilder.BuildAbility();
        }

        public IEnumerable<Ability> GetAbilities()
        {
            foreach (Ability ability in abilities)
            {
                yield return ability;
            }
        }

        public int GetCount()
        {
            return abilities.Count;
        }

        public void GrantAbility(string key, Ability ability)
        {
            if (identifierToAbility.TryGetValue(key, out Ability foundAbility))
            {
                abilities.Remove(foundAbility);
                identifierToAbility.Remove(key);
            }
            identifierToAbility.Add(key, ability);
            abilities.Add(ability);
        }

        public void RevokeAbility(string key)
        {
            if (identifierToAbility.TryGetValue(key, out Ability foundAbility))
            {
                abilities.Remove(foundAbility);
                identifierToAbility.Remove(key);
            }
        }

        public Ability GetRandomAbility()
        {
            int random = Random.Range(0, abilities.Count);
            return abilities[random];
        }

        public bool ValidAbility(Ability ability)
        {
            return ability.primaryAbilityAction.IsValid(toolManager);
        }
    }
}
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;

public class AbilityHolderConfiguration : SerializedScriptableObject
{
    [OdinSerialize]
    private AbilitySO attackAbility;
    [OdinSerialize]
    private AbilitySO defendAbility;
    [OdinSerialize]
    private List<AbilitySO> defaultAbilities;

    public List<AbilitySO> DefaultAbilities
    {
        get
        {
            if (defaultAbilities == null)
            {
                if (this == DefaultValues.Instance.defaultAbilityHolderConfiguration)
                {
                    defaultAbilities = new List<AbilitySO>();
                    return defaultAbilities;
                }
                return DefaultValues.Instance.defaultAbilityHolderConfiguration.DefaultAbilities;
            }
            return defaultAbilities;
        }
    }

    public AbilitySO AttackAbility
    {
        get
        {
            if (attackAbility == null)
            {
                if (this == DefaultValues.Instance.defaultAbilityHolderConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultAbilityHolderConfiguration.attackAbility;
            }
            return attackAbility;
        }
    }

    public AbilitySO DefendAbility
    {
        get
        {
            if (defendAbility == null)
            {
                if (this == DefaultValues.Instance.defaultAbilityHolderConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultAbilityHolderConfiguration.defendAbility;
            }
            return defendAbility;
        }
    }
}

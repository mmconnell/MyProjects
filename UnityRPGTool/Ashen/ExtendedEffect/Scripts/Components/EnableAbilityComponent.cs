using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Manager;

public class EnableAbilityComponent : A_SimpleComponent
{
    private AbilitySO ability;

    public EnableAbilityComponent(AbilitySO ability)
    {
        this.ability = ability;
    }

    public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
    {
        DeliveryTool deliveryTool = dse.target as DeliveryTool;
        AbilityHolder abilityHolder = deliveryTool.toolManager.Get<AbilityHolder>();
        abilityHolder.GrantAbility(dse.key, ability.abilityBuilder.BuildAbility());
    }

    public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
    {
        DeliveryTool deliveryTool = dse.target as DeliveryTool;
        AbilityHolder abilityHolder = deliveryTool.toolManager.Get<AbilityHolder>();
        abilityHolder.RevokeAbility(dse.key);
    }
}

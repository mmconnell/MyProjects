using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Manager;

public class EnableAbilityComponent : A_SimpleComponent
{
    private A_Ability ability;

    private bool valid = false;

    public EnableAbilityComponent(A_Ability ability)
    {
        this.ability = ability;
    }

    public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
    {
        DeliveryTool deliveryTool = dse.target as DeliveryTool;
        AbilityHolder abilityHolder = deliveryTool.toolManager.Get<AbilityHolder>();
        valid = abilityHolder.GrantAbility(ability);
    }

    public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
    {
        if (valid)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            AbilityHolder abilityHolder = deliveryTool.toolManager.Get<AbilityHolder>();
            abilityHolder.RevokeAbility(ability);
        }
    }
}

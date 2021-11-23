using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class SelfTargetHolder : A_TargetHolder<SelfTargetHolder>
{
    private bool resolvedTarget;

    public override void GetRandomTargetable(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {}

    public override void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {}

    public override bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        return !resolvedTarget;
    }

    public override void InitializeTarget(I_Targetable targetable, A_PartyManager sourceParty, A_PartyManager targetParty, PlayerInputState inputState)
    {
        inputState.chosenTarget = sourceParty.GetTargetable(sourceParty.GetPosition(inputState.currentlySelected)); 
    }

    public override void Initialize()
    {
        base.Initialize();
        resolvedTarget = false;
    }

    public override I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        resolvedTarget = true;
        ListActionBundle actions = new ListActionBundle();
        actions.Bundles.Add(new SubactionProcessor()
        {
            actionExecutable = new ActionExecutable()
            {
                builder = ability.GetDeliveryPack(),
                source = source,
                target = source,
                sourceAbility = ability,
            }
        });
        return actions;
    }

    public override void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        inputState.nextTarget = null;
        inputState.currentTarget = sourceParty.GetTargetable(sourceParty.GetPosition(inputState.currentlySelected));
        inputState.chosenTarget = inputState.currentTarget;
    }
}

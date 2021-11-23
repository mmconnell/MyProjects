using Ashen.DeliverySystem;
using Manager;
using System.Collections;
using System.Collections.Generic;

public class PartyTargetHolder : A_TargetHolder<PartyTargetHolder>
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

    public override void Initialize()
    {
        base.Initialize();
        resolvedTarget = false;
    }

    public override I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        resolvedTarget = true;
        ListActionBundle actions = new ListActionBundle();
        List<PartyPosition> validPositions = GetValidPositions(source, sourceParty, targetParty, ability);
        foreach (PartyPosition position in targetParty.GetActivePositions())
        {
            if (!validPositions.Contains(position))
            {
                continue;
            }
            ToolManager manager = targetParty.GetToolManager((int)position);
            actions.Bundles.Add(new SubactionProcessor()
            {
                actionExecutable = new ActionExecutable()
                {
                    builder = ability.GetDeliveryPack(),
                    source = source,
                    target = manager,
                    sourceAbility = ability,
                }
            });
        }
        return actions;
    }

    public override void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        if (inputState.nextTarget == null || inputState.currentTarget != null || inputState.nextTarget == inputState.currentTarget)
        {
            inputState.nextTarget = null;
            return;
        }
        List<PartyPosition> validPositions = GetValidPositions(inputState.currentlySelected, sourceParty, targetParty, actionHolder.sourceAbility);
        
        ToolManager nextTargetTM = inputState.nextTarget.GetTarget();
        PartyPosition nextPosition = targetParty.GetPosition(nextTargetTM);

        if (!validPositions.Contains(nextPosition))
        {
            inputState.nextTarget = null;
            return;
        }

        inputState.currentTarget = inputState.nextTarget;

        foreach (PartyPosition position in targetParty.GetActivePositions())
        {
            if (validPositions.Contains(position))
            {
                targetParty.GetTargetable(position).Selected();
            }
        }
    }

    protected override void CleanupInternal(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        foreach (PartyPosition position in targetParty.GetActivePositions())
        {
            targetParty.GetTargetable(position).Deselected();
        }
    }
}

using Manager;
using Ashen.DeliverySystem;
using System.Collections;
using System.Collections.Generic;

public class SingleTargetHolder : A_TargetHolder<SingleTargetHolder>
{
    private I_Targetable target;

    private bool resolvedTarget;

    public override void GetRandomTargetable(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {
        if (targetParty.HasActivePositionsInRow(PartyRow.FRONT))
        {
            PartyPosition pos = targetParty.GetRandomInRow(PartyRow.FRONT);
            target = targetParty.GetTargetable(pos);
        }
        else
        {
            PartyPosition pos = targetParty.GetRandomInRow(PartyRow.BACK);
            target = targetParty.GetTargetable(pos);
        }
    }

    public override void SetTargetable(ToolManager source, ToolManager target, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionProcessor)
    {
        PartyPosition position = targetParty.GetPosition(target);
        this.target = targetParty.GetTargetable(position);
    }

    public override void Initialize()
    {
        resolvedTarget = false;
    }

    public override bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        return !resolvedTarget;
    }

    public override I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        ToolManager primaryManager = target.GetTarget();
        PartyPosition primaryPosition = targetParty.GetPosition(primaryManager);

        List<PartyPosition> validPositions = GetValidPositions(source, sourceParty, targetParty, ability);

        if (!validPositions.Contains(primaryPosition))
        {
            primaryPosition = null;
        }

        if (primaryPosition == null)
        {
            primaryManager = null;
            primaryPosition = null;
            if (targetParty.HasActivePositionsInRow(PartyRow.FRONT))
            {
                primaryPosition = targetParty.GetRandomInRow(PartyRow.FRONT);
                primaryManager = targetParty.GetToolManager((int)primaryPosition);
            }
            else
            {
                primaryPosition = targetParty.GetRandomInRow(PartyRow.BACK);
                primaryManager = targetParty.GetToolManager((int)primaryPosition);
            }
            if (!validPositions.Contains(primaryPosition))
            {
                return new ListActionBundle();
            }
        }

        SubactionProcessor action = new SubactionProcessor();
        action.actionExecutable = new ActionExecutable
        {
            builder = ability.GetDeliveryPack(),
            target = primaryManager,
            source = source,
            sourceAbility = ability,
        };
        if (ability.GetAnimation() != null)
        {
            AnimationCenterTracker tracker = primaryManager.Get<AnimationCenterTracker>();
            action.animationExecutable = new AnimationExecutable
            {
                animation = ability.GetAnimation(),
                location = tracker.animationCenter.transform.position,
                waitTime = 0.3f,
                position = primaryPosition,
            };
        }

        resolvedTarget = true;

        return action;
    }

    public override void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        if (inputState.nextTarget == null || inputState.currentTarget == inputState.nextTarget)
        {
            inputState.nextTarget = null;
            return;
        }
        List<PartyPosition> validPositions = GetValidPositions(inputState.currentlySelected, sourceParty, targetParty, actionHolder.sourceAbility);
        PartyPosition nextPosition = targetParty.GetPosition(inputState.nextTarget.GetTarget());
        if (!validPositions.Contains(nextPosition))
        {
            inputState.nextTarget = null;
            return;
        }
        inputState.nextTarget.Selected();
        if (inputState.currentTarget != null)
        {
            inputState.currentTarget.Deselected();
        }
        inputState.currentTarget = inputState.nextTarget;
        target = inputState.currentTarget;
        inputState.nextTarget = null;
    }

    public override void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {
        PartyPosition position = this.GetPositionByThreat(source, sourceParty, targetParty, actionHolder.sourceAbility);
        target = targetParty.GetTargetable(position);
    }
}

using Manager;
using UnityEngine.EventSystems;
using Ashen.DeliverySystem;
using System.Collections;
using System.Collections.Generic;

public class RowTargetHolder : A_TargetHolder<RowTargetHolder>
{
    private PartyRow currentRow;

    private bool resolvedTarget;

    public override void Initialize()
    {
        resolvedTarget = false;
    }

    public override void GetRandomTargetable(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {
        currentRow = targetParty.GetRandomRow();
    }

    public override void SetTargetable(ToolManager source, ToolManager target, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionProcessor)
    {
        PartyPosition position = targetParty.GetPosition(target);
        this.currentRow = position.row;
    }

    public override I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        resolvedTarget = true;
        ListActionBundle actions = new ListActionBundle();
        if (!targetParty.HasActivePositionsInRow(currentRow))
        {
            currentRow = currentRow == PartyRow.FRONT ? PartyRow.BACK : PartyRow.FRONT;
        }
        List<PartyPosition> validPositions = GetValidPositions(source, sourceParty, targetParty, ability);
        foreach (PartyPosition position in targetParty.GetActivePositionsInRow(currentRow))
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
                    target = manager,
                    source = source,
                    sourceAbility = ability,
                }
            });
        }
        return actions;
    }

    public override void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        inputState.moveDirection = MoveDirection.None;
        if (inputState.nextTarget == null || inputState.currentTarget == inputState.nextTarget)
        {
            inputState.nextTarget = null;
            return;
        }
        if (inputState.currentTarget == null)
        {
            inputState.currentTarget = inputState.nextTarget;
            inputState.nextTarget = null;
            PartyPosition position = targetParty.GetPosition(inputState.currentTarget.GetTarget());
            SelectAllForRow(position, targetParty, inputState);
            return;
        }
        List<PartyPosition> validPositions = GetValidPositions(inputState.currentlySelected, sourceParty, targetParty, actionHolder.sourceAbility);
        PartyPosition nextPosition = targetParty.GetPosition(inputState.nextTarget.GetTarget());
        if (!validPositions.Contains(nextPosition))
        {
            inputState.nextTarget = null;
            return;
        }
        if (nextPosition.row == currentRow)
        {
            inputState.nextTarget = null;
            return;
        }
        SelectAllForRow(nextPosition, targetParty, inputState);
        inputState.currentTarget = inputState.nextTarget;
        inputState.nextTarget = null;
    }

    private void SelectAllForRow(PartyPosition position, A_PartyManager targetParty, PlayerInputState inputState)
    {
        foreach (PartyPosition prevPosition in targetParty.GetActivePositionsInRow(currentRow))
        {
            I_Targetable targetable = targetParty.GetTargetable(prevPosition);
            if (targetable != null)
            {
                targetable.Deselected();
            }
        }

        currentRow = position.row;

        foreach (PartyPosition nextPosition in targetParty.GetActivePositionsInRow(currentRow))
        {
            I_Targetable targetable = targetParty.GetTargetable(nextPosition);
            if (targetable != null)
            {
                targetable.Selected();
            }
        }
    }

    protected override void CleanupInternal(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        foreach (PartyPosition prevPosition in targetParty.GetActivePositionsInRow(currentRow))
        {
            I_Targetable targetable = targetParty.GetTargetable(prevPosition);
            if (targetable != null)
            {
                targetable.Deselected();
            }
        }
    }

    public override bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        return !resolvedTarget;
    }

    public override void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {
        PartyPosition position = this.GetPositionByThreat(source, sourceParty, targetParty, actionHolder.sourceAbility);
        currentRow = position.row;
    }
}

using Manager;
using UnityEngine.EventSystems;
using Ashen.DeliverySystem;
using System.Collections;
using System.Collections.Generic;

public class PiercingTargetHolder : A_TargetHolder<PiercingTargetHolder>
{
    public float backDamageRatio;

    private I_Targetable primary;
    private I_Targetable backTarget;

    private bool resolvedTarget;

    public override void Initialize()
    {
        resolvedTarget = false;
    }

    public override I_TargetHolder Clone()
    {
        return new PiercingTargetHolder
        {
            backDamageRatio = backDamageRatio
        };
    }

    public override void GetRandomTargetable(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {
        ToolManager primaryManager = null;
        PartyPosition primaryPosition = null;

        resolvedTarget = true;
        
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

        primary = targetParty.GetTargetable(primaryPosition);

        if (primaryPosition.row == PartyRow.BACK)
        {
            return;
        }

        if (targetParty.HasActivePositionsInRow(PartyRow.BACK))
        {
            PartyPosition backPosition = targetParty.GetRandomInRow(PartyRow.BACK);
            ToolManager backManager = targetParty.GetToolManager((int)backPosition);
            backTarget = targetParty.GetTargetable(backPosition);
        }
    }

    public override void SetTargetable(ToolManager source, ToolManager target, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionProcessor)
    {
        PartyPosition position = targetParty.GetPosition(target);
        this.primary = targetParty.GetTargetable(position);
    }

    public override I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        resolvedTarget = true;

        ToolManager primaryManager = primary.GetTarget();
        PartyPosition primaryPosition = targetParty.GetPosition(primaryManager);

        ListActionBundle actions = new ListActionBundle();

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
        }

        actions.Bundles.Add(new SubactionProcessor()
        {
            actionExecutable = new ActionExecutable()
            {
                builder = ability.GetDeliveryPack(),
                target = primaryManager,
                source = source,
                sourceAbility = ability,
            }
        });

        if (primaryPosition.row == PartyRow.BACK)
        {
            return actions;
        }

        ToolManager backManager = null;
        PartyPosition backPosition = null;

        if (backTarget != null)
        {
            backManager = backTarget.GetTarget();
            backPosition = targetParty.GetPosition(backManager);
        }

        if (backManager == null || backPosition == null || backPosition.row != PartyRow.BACK)
        {
            backManager = null;
            backPosition = null;
            if (targetParty.HasActivePositionsInRow(PartyRow.BACK))
            {
                backPosition = targetParty.GetRandomInRow(PartyRow.BACK);
                backManager = targetParty.GetToolManager((int)backPosition);
            }
            else
            {
                return actions;
            }
        }
        float?[] effectFloatArguments = new float?[EffectFloatArguments.Count];
        effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] = this.backDamageRatio;
        actions.Bundles.Add(new SubactionProcessor()
        {
            actionExecutable = new ActionExecutable()
            {
                builder = ability.GetDeliveryPack(),
                target = backManager,
                source = source,
                sourceAbility = ability,
                effectFloatArguments = effectFloatArguments,
            }
        });

        return actions;
    }

    public override void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        if (inputState.nextTarget == null)
        {
            return;
        }
        List<PartyPosition> validPositions = GetValidPositions(inputState.currentlySelected, sourceParty, targetParty, actionHolder.sourceAbility);

        ToolManager nextTool = inputState.nextTarget.GetTargets()[0];
        PartyPosition nextPosition = targetParty.GetPosition(nextTool);

        if (!validPositions.Contains(nextPosition))
        {
            inputState.nextTarget = null;
            return;
        }

        if (inputState.currentTarget == null)
        {
            InitializeFrontTarget(nextPosition, targetParty, inputState, validPositions);
            return;
        }

        ToolManager currentTool = inputState.currentTarget.GetTargets()[0];
        PartyPosition currentPosition = targetParty.GetPosition(currentTool);

        if (currentPosition.row == PartyRow.BACK)
        {
            if (nextPosition.row == PartyRow.FRONT)
            {
                inputState.currentTarget.Deselected();
                InitializeFrontTarget(nextPosition, targetParty, inputState, validPositions);
                return;
            }
            inputState.nextTarget.Selected();
            inputState.currentTarget.Deselected();
            inputState.currentTarget = inputState.nextTarget;
            primary = inputState.currentTarget;
            inputState.nextTarget = null;
            return;
        }

        if (nextPosition.row == PartyRow.BACK)
        {
            if (backTarget != null)
            {
                backTarget.Deselected();
                backTarget = null;
            }
            inputState.nextTarget.Selected();
            inputState.currentTarget.Deselected();
            inputState.currentTarget = inputState.nextTarget;
            primary = inputState.currentTarget;
            inputState.nextTarget = null;
            return;
        }

        if (backTarget == null)
        {
            inputState.nextTarget.Selected();
            inputState.currentTarget.Deselected();
            inputState.currentTarget = inputState.nextTarget;
            primary = inputState.currentTarget;
            inputState.nextTarget = null;
            return;
        }

        ToolManager backTool = backTarget.GetTarget();
        PartyPosition backPosition = targetParty.GetPosition(backTool);

        if (MoveDirection.Left == inputState.moveDirection)
        {
            PartyPosition previous = targetParty.GetPreviousTargetableCharacterInRow(backPosition);
            if (previous != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(inputState.currentTarget.GetSelectableObject().gameObject);
                inputState.nextTarget = null;
                backTarget.Deselected();
                backTarget = targetParty.GetTargetable(previous);
                backTarget.SelectedSecondary();
                return;
            }
            if (backTarget != null)
            {
                backTarget.Deselected();
            }
            backTarget = targetParty.GetLastTargetableCharacterInRow(PartyRow.BACK);
            if (backTarget != null)
            {
                backTarget.SelectedSecondary();
            }
        }
        else if (MoveDirection.Right == inputState.moveDirection)
        {
            PartyPosition next = targetParty.GetNextTargetableCharacterInRow(backPosition);
            if (next != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(inputState.currentTarget.GetSelectableObject().gameObject);
                inputState.nextTarget = null;
                backTarget.Deselected();
                backTarget = targetParty.GetTargetable(next);
                backTarget.SelectedSecondary();
                return;
            }
            if (backTarget != null)
            {
                backTarget.Deselected();
            }
            backTarget = targetParty.GetFirstTargetableCharacterInRow(PartyRow.BACK);
            if (backTarget != null)
            {
                backTarget.SelectedSecondary();
            }
        }

        inputState.nextTarget.Selected();
        inputState.currentTarget.Deselected();
        inputState.currentTarget = inputState.nextTarget;
        primary = inputState.currentTarget;
        inputState.nextTarget = null;
    }

    private void InitializeFrontTarget(PartyPosition position, A_PartyManager party, PlayerInputState inputState, List<PartyPosition> validPositions)
    {
        inputState.nextTarget.Selected();
        inputState.currentTarget = inputState.nextTarget;
        primary = inputState.currentTarget;
        inputState.nextTarget = null;
        if (position.row == PartyRow.FRONT)
        {
            backTarget = party.GetFirstTargetableCharacterInRow(PartyRow.BACK);
            
            if (backTarget != null)
            {
                if (!validPositions.Contains(party.GetPosition(backTarget.GetTarget()))) {
                    backTarget = null;
                    return;
                }
                backTarget.SelectedSecondary();
            }
        }
    }

    protected override void CleanupInternal(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        if (backTarget != null)
        {
            backTarget.Deselected();
        }
    }

    public override bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        return !resolvedTarget;
    }

    public override void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {
        PartyPosition position = this.GetPositionByThreat(source, sourceParty, targetParty, actionHolder.sourceAbility);
        primary = targetParty.GetTargetable(position);
    }
}

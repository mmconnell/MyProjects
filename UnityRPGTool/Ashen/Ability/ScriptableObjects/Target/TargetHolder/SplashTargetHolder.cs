using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;
using System.Collections.Generic;

public class SplashTargetHolder : A_TargetHolder<SplashTargetHolder>
{
    public float splashDamageRatio = 1f;

    private I_Targetable target;

    private bool resolvedTarget;

    public override void Initialize()
    {
        resolvedTarget = false;
    }

    public override I_TargetHolder Clone()
    {
        return new SplashTargetHolder
        {
            splashDamageRatio = this.splashDamageRatio
        };
    }

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

    public override bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        return !resolvedTarget;
    }

    public override I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        resolvedTarget = true;
        ListActionBundle actions = new ListActionBundle();

        ToolManager primaryManager = target.GetTarget();
        PartyPosition primaryPosition = targetParty.GetPosition(primaryManager);

        List<PartyPosition> validPositions = GetValidPositions(source, sourceParty, targetParty, ability);
        if (!validPositions.Contains(primaryPosition))
        {
            return new ListActionBundle();
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
        }

        actions.Bundles.Add(new SubactionProcessor() {
            actionExecutable = new ActionExecutable()
            {
                builder = ability.GetDeliveryPack(),
                source = source,
                target = primaryManager,
                sourceAbility = ability,
            }
        });

        PartyPosition leftPosition = targetParty.GetPreviousTargetableCharacterInRow(primaryPosition);
        float?[] effectFloatArguments = new float?[EffectFloatArguments.Count];
        effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] = this.splashDamageRatio;
        if (leftPosition != null)
        {
            ToolManager leftManager = targetParty.GetToolManager(leftPosition);
            DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
            EffectsArgumentPack effectsPacks = packs.GetPack<EffectsArgumentPack>();
            effectsPacks.SetFloatArgument(EffectFloatArguments.Instance.reservedDamageScale, this.splashDamageRatio);
            actions.Bundles.Add(new SubactionProcessor()
            {
                actionExecutable = new ActionExecutable()
                {
                    builder = ability.GetDeliveryPack(),
                    source = source,
                    target = leftManager,
                    sourceAbility = ability,
                    effectFloatArguments = effectFloatArguments,
                }
            });
        }

        PartyPosition rightPosition = targetParty.GetNextTargetableCharacterInRow(primaryPosition);

        if (rightPosition != null)
        {
            ToolManager rightManager = targetParty.GetToolManager(rightPosition);
            DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
            EffectsArgumentPack effectsPacks = packs.GetPack<EffectsArgumentPack>();
            effectsPacks.SetFloatArgument(EffectFloatArguments.Instance.reservedDamageScale, this.splashDamageRatio);
            actions.Bundles.Add(new SubactionProcessor()
            {
                actionExecutable = new ActionExecutable()
                {
                    builder = ability.GetDeliveryPack(),
                    source = source,
                    target = rightManager,
                    sourceAbility = ability,
                    effectFloatArguments = effectFloatArguments,
                }
            });
        }

        if (ability.GetAnimation() != null)
        {
            AnimationCenterTracker tracker = primaryManager.Get<AnimationCenterTracker>();
            actions.Bundles.Add(new StandaloneAnimationBundle()
            {
                animationExecutable = new AnimationExecutable()
                {
                    animation = ability.GetAnimation(),
                    location = tracker.animationCenter.transform.position,
                    waitTime = 0.3f,
                    position = primaryPosition,
                }
            });
        }

        return actions;
    }

    public override void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        if (inputState.nextTarget == null || inputState.currentTarget == inputState.nextTarget)
        {
            inputState.nextTarget = null;
            return;
        }

        List<PartyPosition> validPositions = GetValidPositions(inputState.currentlySelected, sourceParty, targetParty, actionHolder.sourceAbility);
        ToolManager primary = inputState.nextTarget.GetTarget();
        PartyPosition primaryPosition = targetParty.GetPosition(primary);
        if (!validPositions.Contains(primaryPosition))
        {
            inputState.nextTarget = null;
            return;
        }
        if (inputState.currentTarget != null)
        {
            targetParty.DeselectAll();
        }

        inputState.nextTarget.Selected();

        PartyPosition leftPosition = targetParty.GetPreviousTargetableCharacterInRow(primaryPosition);

        if (leftPosition != null)
        {
            I_Targetable leftTarget = targetParty.GetTargetable(leftPosition);
            leftTarget.SelectedSecondary();
        }

        PartyPosition rightPosition = targetParty.GetNextTargetableCharacterInRow(primaryPosition);

        if (rightPosition != null)
        {
            I_Targetable rightTarget = targetParty.GetTargetable(rightPosition);
            rightTarget.SelectedSecondary();
        }

        inputState.currentTarget = inputState.nextTarget;
        target = inputState.currentTarget;
        inputState.nextTarget = null;
    }

    protected override void CleanupInternal(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        ToolManager previous = target.GetTarget();
        PartyPosition previousPos = targetParty.GetPosition(previous);

        PartyPosition leftPrevPos = targetParty.GetPreviousTargetableCharacterInRow(previousPos);

        if (leftPrevPos != null)
        {
            targetParty.GetTargetable(leftPrevPos).Deselected();
        }

        PartyPosition rightPrevPos = targetParty.GetNextTargetableCharacterInRow(previousPos);

        if (rightPrevPos != null)
        {
            targetParty.GetTargetable(rightPrevPos).Deselected();
        }
    }

    public override void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {
        PartyPosition position = this.GetPositionByThreat(source, sourceParty, targetParty, actionHolder.sourceAbility);
        target = targetParty.GetTargetable(position);
    }
}

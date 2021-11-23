using UnityEngine;
using System.Collections;
using Manager;
using Ashen.VariableSystem;
using Ashen.EquationSystem;
using System.Collections.Generic;

public class RandomStrikesTargetHolder : A_TargetHolder<RandomStrikesTargetHolder>
{
    [SerializeField]
    private int minHits;
    [SerializeField]
    private int maxHits;
    [SerializeField, Range(0, 100)]
    private int decayStart;
    [SerializeField, Range(0, 100)]
    private int decayRate;

    private int decay;
    private int targetCounter;

    public override void Initialize()
    {
        decay = decayStart;
        targetCounter = 0;
    }

    public override void GetRandomTargetable(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {}

    public override I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        if (targetCounter >= minHits)
        {
            decay -= decayRate;
        }
        List<PartyPosition> validPositions = GetValidPositions(source, sourceParty, targetParty, ability);

        ToolManager manager = targetParty.GetRandom(validPositions);
        PartyPosition position = targetParty.GetPosition(manager);
        targetCounter++;
        SubactionProcessor action = new SubactionProcessor();
        action.actionExecutable = new ActionExecutable
        {
            builder = ability.GetDeliveryPack(),
            target = manager,
            source = source,
            sourceAbility = ability,
        };
        if (ability.GetAnimation() != null)
        {
            AnimationCenterTracker tracker = manager.Get<AnimationCenterTracker>();
            action.animationExecutable = new AnimationExecutable
            {
                animation = ability.GetAnimation(),
                location = tracker.animationCenter.transform.position,
                waitTime = 0.3f,
                position = position,
            };
        }
        return action;
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

    public override I_TargetHolder Clone()
    {
        return new RandomStrikesTargetHolder
        {
            decayRate = decayRate,
            decayStart = decayStart,
            maxHits = maxHits,
            minHits = minHits
        };
    }

    public override bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        return targetCounter <= maxHits && (targetCounter < minHits || Random.Range(0, 100) < decay);
    }

    public override void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder)
    {}
}

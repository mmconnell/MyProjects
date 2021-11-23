using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static AbilityBuilder;

public class ChooseTarget : I_GameState
{
    private I_GameState previousState;
    private Ability ability;

    public ChooseTarget(I_GameState sourceState, Ability ability)
    {
        previousState = sourceState;
        this.ability = ability;
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerInputState inputState = PlayerInputState.Instance;

        I_AbilityAction abilityAction = ability.primaryAbilityAction;

        Target target = abilityAction.GetTargetType(inputState.currentlySelected);
        A_PartyManager targetParty = abilityAction.GetTargetParty() == TargetParty.ALLY ? inputState.sourceParty : EnemyPartyHolder.Instance.enemyPartyManager;
        
        I_TargetHolder targetHolder = target.BuildTargetHolder();
        I_Targetable targetable = targetParty.GetFirstTargetableCharacter();
        ActionProcessor actionHolder = new ActionProcessor(abilityAction, inputState.currentlySelected, inputState.sourceParty, targetParty, targetHolder);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(targetable.GetSelectableObject().gameObject);
        inputState.chosenTarget = null;
        targetHolder.InitializeTarget(targetable, inputState.sourceParty, targetParty, inputState);

        
        while (inputState.chosenTarget == null && !inputState.backRequested)
        {
            targetHolder.ResolveTargetRequest(inputState.sourceParty, targetParty, actionHolder, inputState);
            GameObject selectable = inputState.currentTarget.GetSelectableObject().gameObject;
            if (EventSystem.current.currentSelectedGameObject != selectable)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(selectable);
            }
            yield return null;
        }

        targetHolder.Cleanup(inputState.sourceParty, targetParty, actionHolder, inputState);

        if (inputState.backRequested)
        {
            inputState.backRequested = false;
            response.nextState = previousState;
            yield break;
        }

        List<ActionProcessor> actionProcessors = new List<ActionProcessor>();
        actionProcessors.Add(actionHolder);

        if (ability.secondaryAbilityActions != null)
        {
            foreach (SubAbilityAction subAction in ability.secondaryAbilityActions)
            {
                Target subTarget = subAction.GetTargetType(inputState.currentlySelected);
                A_PartyManager subTargetParty = subAction.GetTargetParty() == TargetParty.ALLY ? inputState.sourceParty : EnemyPartyHolder.Instance.enemyPartyManager;
                I_TargetHolder subTargetHolder = subTarget.BuildTargetHolder();
                ActionProcessor subActionProcessor = new ActionProcessor(subAction, inputState.currentlySelected, inputState.sourceParty, subTargetParty, subTargetHolder);
                if (subAction.relativeTarget == SubAbilityAction.SubAbilityRelativeTarget.Self)
                {
                    subTargetHolder.SetTargetable(inputState.currentlySelected, inputState.currentlySelected, inputState.sourceParty, subTargetParty, subActionProcessor);
                }
                else if (subAction.relativeTarget == SubAbilityAction.SubAbilityRelativeTarget.Target)
                {
                    subTargetHolder.SetTargetable(inputState.currentlySelected, inputState.chosenTarget.GetTarget(), inputState.sourceParty, subTargetParty, subActionProcessor);
                }
                else if (subAction.relativeTarget == SubAbilityAction.SubAbilityRelativeTarget.Random)
                {
                    subTargetHolder.GetRandomTargetable(inputState.currentlySelected, inputState.sourceParty, subTargetParty, subActionProcessor);
                }
                subTargetHolder.SetTargetable(inputState.currentlySelected, inputState.chosenTarget.GetTarget(), inputState.sourceParty, subTargetParty, subActionProcessor);
                if (subAction.relativeSpeed == SubAbilityAction.RelativeSpeed.After)
                {
                    subActionProcessor.speed = actionHolder.speed - .01f;
                }
                else if (subAction.relativeSpeed == SubAbilityAction.RelativeSpeed.Before)
                {
                    subActionProcessor.speed = actionHolder.speed + .01f;
                }
                actionProcessors.Add(subActionProcessor);
            }
        }

        response.nextState = new AddPlayerAction(actionProcessors);
        yield break;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using static AbilitySO;
using Manager;

public class ChooseCombatOption : I_GameState
{
    private CombatOption previousSubmittedCombatOption;
    private CombatOption previousHoveredCombatOption;
    private I_GameState lastTurnNextState;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerInputState inputState = PlayerInputState.Instance;
        ActionOptionsManager optionsManager = ActionOptionsManager.Instance;
        optionsManager.Restart();
        optionsManager.RegisterToolManager(inputState.currentlySelected);
        EventSystem.current.SetSelectedGameObject(null);
        inputState.chosenAbility = null;
        inputState.hoveredCombatOption = null;
        inputState.submittedCombatOption = null;
        previousHoveredCombatOption = null;
        if (previousSubmittedCombatOption != null)
        {
            EventSystem.current.SetSelectedGameObject(optionsManager.GetCombatOptionUI(previousSubmittedCombatOption).gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(optionsManager.GetCombatOptionUI(CombatOptions.Instance.ATTACK).gameObject);
        }
        
        while(true)
        {
            if (inputState.hoveredCombatOption)
            {
                if (previousHoveredCombatOption != null && previousHoveredCombatOption != inputState.hoveredCombatOption)
                {
                    optionsManager.GetCombatOptionUI(previousHoveredCombatOption).GradientEnabled(false);
                }
                optionsManager.GetCombatOptionUI(inputState.hoveredCombatOption).GradientEnabled(true);
                previousHoveredCombatOption = inputState.hoveredCombatOption;
                inputState.hoveredCombatOption = null;
            }
            if (inputState.backRequested)
            {
                if (inputState.sourceParty.GetPrevious(inputState.currentlySelected) == null)
                {
                    inputState.backRequested = false;
                }
                else
                {
                    break;
                }
            }
            if (inputState.submittedCombatOption)
            {
                if (inputState.submittedCombatOption == CombatOptions.Instance.ATTACK
                    || inputState.submittedCombatOption == CombatOptions.Instance.SKILLS
                    || inputState.submittedCombatOption == CombatOptions.Instance.DEFEND
                    )
                {
                    break;
                }
                inputState.submittedCombatOption = null;
            }
            yield return null;

        }

        if (inputState.backRequested)
        {
            inputState.backRequested = false;
            response.nextState = new MovePreviousTurn();
            yield break;
        }

        CombatOption selectedOption = inputState.submittedCombatOption;
        inputState.submittedCombatOption = null;

        if (lastTurnNextState != null && previousSubmittedCombatOption == selectedOption)
        {
            response.nextState = lastTurnNextState;
            yield break;
        }

        if (selectedOption == CombatOptions.Instance.ATTACK)
        {
            AbilityHolder abilityHolder = inputState.currentlySelected.Get<AbilityHolder>();

            response.nextState = new ChooseTarget(this, abilityHolder.AttackAbility);
            lastTurnNextState = response.nextState;
            previousSubmittedCombatOption = selectedOption;
            yield break;
        }
        else if (selectedOption == CombatOptions.Instance.SKILLS)
        {
            response.nextState = new ChooseSkill(this);
            lastTurnNextState = response.nextState;
            previousSubmittedCombatOption = selectedOption;
            yield break;
        }
        else if (selectedOption == CombatOptions.Instance.DEFEND)
        {
            AbilityHolder abilityHolder = inputState.currentlySelected.Get<AbilityHolder>();

            response.nextState = new ChooseTarget(this, abilityHolder.DefendAbility);
            lastTurnNextState = response.nextState;
            previousSubmittedCombatOption = selectedOption;
            yield break;
        }
 
        //inputState.targetParty = inputState.chosenAbility.targetParty == TargetParty.PLAYER ? inputState.sourceParty : EnemyPartyHolder.Instance.enemyPartyManager;
        //inputState.actionHolder = new ActionHolder(inputState.chosenAbility, inputState.currentlySelected, inputState.sourceParty, inputState.targetParty);
        //Target target = inputState.chosenAbility.GetTargetType(inputState.currentlySelected);
        //if (target == null)
        //{
        //    response.nextState = new AddPlayerAction();
        //    optionsManager.previouslySelected = optionsManager.currentlySelected;
        //    yield break;
        //}
        //I_TargetHolder targetHolder = target.BuildTargetHolder();
        //inputState.actionHolder.targetHolder = targetHolder;
        //response.nextState = new ChooseTarget();
        //optionsManager.previouslySelected = optionsManager.currentlySelected;
        yield break;
    }
}

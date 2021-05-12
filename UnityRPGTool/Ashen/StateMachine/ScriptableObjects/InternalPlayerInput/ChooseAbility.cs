using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ChooseAbility : I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        ActionOptionsManager optionsManager = ActionOptionsManager.Instance;
        optionsManager.Restart();
        EventSystem.current.SetSelectedGameObject(null);
        if (optionsManager.previouslySelected != null)
        {
            EventSystem.current.SetSelectedGameObject(optionsManager.previouslySelected.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(optionsManager.first.gameObject);
        }
        PlayerInputState inputState = PlayerInputState.Instance;
        inputState.chosenAbility = null;
        while(inputState.chosenAbility == null && !inputState.backRequested)
        {
            yield return null;
        }
        if (inputState.backRequested)
        {
            inputState.backRequested = false;
            if (inputState.sourceParty.GetPrevious(inputState.currentlySelected) == null)
            {
                response.nextState = new ChooseAbility();
                yield break;
            }
            response.nextState = new MovePreviousTurn();
            yield break;
        }

        inputState.targetParty = inputState.chosenAbility.targetParty == TargetParty.PLAYER ? inputState.sourceParty : EnemyPartyHolder.Instance.enemyPartyManager;
        inputState.actionHolder = new ActionHolder(inputState.chosenAbility, inputState.currentlySelected, inputState.sourceParty, inputState.targetParty);
        Target target = inputState.chosenAbility.GetTargetType();
        if (target == null)
        {
            response.nextState = new AddPlayerAction();
            optionsManager.previouslySelected = optionsManager.currentlySelected;
            yield break;
        }
        I_TargetHolder targetHolder = target.BuildTargetHolder();
        I_Targetable targetable = targetHolder.GetTargetable(inputState.sourceParty, inputState.targetParty, inputState.actionHolder);
        inputState.actionHolder.targetHodler = targetHolder;
        response.nextState = new ChooseTarget();
        optionsManager.previouslySelected = optionsManager.currentlySelected;
        yield break;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ChooseTarget : I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerInputState inputState = PlayerInputState.Instance;

        Target target = inputState.chosenAbility.GetTargetType();

        I_TargetHolder targetHolder = target.BuildTargetHolder();
        I_Targetable targetable = targetHolder.GetTargetable(inputState.sourceParty, inputState.targetParty, inputState.actionHolder);
        inputState.actionHolder.targetHodler = targetHolder;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(targetable.GetSelectableObject().gameObject);
        inputState.chosenTarget = null;
        while (inputState.chosenTarget == null && !inputState.backRequested)
        {
            yield return null;
        }
        if (inputState.backRequested)
        {
            inputState.backRequested = false;
            response.nextState = new ChooseAbility();
            yield break;
        }
        targetHolder.SetTarget(inputState.chosenTarget);
        response.nextState = new AddPlayerAction();
        yield break;
    }
}

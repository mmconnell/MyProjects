using JoshH.UI;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputState : SingletonScriptableObject<PlayerInputState>, I_GameState
{
    public GameStateManager internalPlayerInputManager;

    [NonSerialized]
    public A_Ability chosenAbility;
    private I_Targetable selectedTarget;
    public I_Targetable SelectedTarget
    {
        get
        {
            return selectedTarget;
        }
        set
        {
            if (selectedTarget != null)
            {
                chosenAbility.ProcessTargetDisplay(sourceParty, targetParty, actionHolder, selectedTarget, value);
            }
            selectedTarget = value;
        }
    }
    [NonSerialized]
    public I_Targetable chosenTarget;
    [NonSerialized]
    public A_PartyManager sourceParty;
    [NonSerialized]
    public A_PartyManager targetParty;
    [NonSerialized]
    public ActionHolder actionHolder;
    [NonSerialized]
    public ToolManager currentlySelected;
    [NonSerialized]
    public bool backRequested = false;
    [NonSerialized]
    public bool movePrevious = false;

    public void Reset()
    {
        chosenAbility = null;
        selectedTarget = null;
        chosenTarget = null;
        sourceParty = PlayerPartyHolder.Instance.partyManager;
        targetParty = null;
        actionHolder = null;
        backRequested = false;
        movePrevious = false;
        ActionOptionsManager.Instance.previouslySelected = null;
    }
    
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        Reset();

        A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
        PartyUIManager playerUiManager = PartyUIManager.Instance;
        ExecuteInputState executeInputState = ExecuteInputState.Instance;
        EnemyPartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;

        ActionOptionsManager.Instance.gameObject.SetActive(true);
        currentlySelected = playerParty.GetFirst();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ActionOptionsManager.Instance.first.gameObject);
        if (currentlySelected != null)
        {
            A_CharacterSelector manager = PartyUIManager.Instance.positionToManager[playerParty.GetPosition(currentlySelected)];
            manager.GetComponent<UIGradient>().enabled = false;
        }
        while (currentlySelected != null)
        {
            Reset();
            yield return internalPlayerInputManager.RunState(request, new GameStateResponse());
            if (movePrevious)
            {
                ToolManager previous = playerParty.GetPrevious(currentlySelected);
                if (previous != null)
                {
                    ChangeTurn(playerParty, playerUiManager, currentlySelected, previous);
                    executeInputState.ClearActions(previous);
                    currentlySelected = previous;
                    continue;
                }
            }
            ToolManager next = playerParty.GetNext(currentlySelected);
            if (next != null)
            {
                ChangeTurn(playerParty, playerUiManager, currentlySelected, next);
                currentlySelected = next;
                continue;
            }
            ChangeTurn(playerParty, playerUiManager, currentlySelected, null);
            currentlySelected = null;
            EventSystem.current.SetSelectedGameObject(null);
            //while (chosenAbility == null && !backRequested)
            //{
            //    yield return null;
            //}
            //targetParty = chosenAbility.targetParty == TargetParty.PLAYER ? playerParty : enemyParty;
            //actionHolder = new ActionHolder(chosenAbility, currentlySelected, sourceParty, targetParty);
            //Target target = chosenAbility.GetTargetType();
            //I_TargetHolder targetHolder = target.BuildTargetHolder();
            //I_Targetable targetable = targetHolder.GetTargetable(sourceParty, targetParty, actionHolder);
            //actionHolder.targetHodler = targetHolder;
            //EventSystem.current.SetSelectedGameObject(null);
            //EventSystem.current.SetSelectedGameObject(targetable.GetSelectableObject().gameObject);
            //while (chosenTarget == null)
            //{
            //    yield return null;
            //}
            //targetHolder.SetTarget(chosenTarget);
            //executeInputState.AddCombatAction(actionHolder);
            //if (currentlySelected != null)
            //{
            //    A_CharacterSelector manager = playerUiManager.positionToManager[playerParty.GetPosition(currentlySelected)];
            //    manager.GetComponent<UIGradient>().enabled = true;
            //}
            //currentlySelected = playerParty.GetNext(currentlySelected);
            //EventSystem.current.SetSelectedGameObject(null);
            //EventSystem.current.SetSelectedGameObject(ActionOptionsManager.Instance.first);
            //if (currentlySelected != null)
            //{
            //    A_CharacterSelector manager = playerUiManager.positionToManager[playerParty.GetPosition(currentlySelected)];
            //    manager.GetComponent<UIGradient>().enabled = false;
            //}
        }
        ActionOptionsManager.Instance.gameObject.SetActive(false);
        yield return null;
        Reset();
        response.nextState = EnemyInputState.Instance;
    }

    private void ChangeTurn(A_PartyManager party, PartyUIManager playerUiManager, ToolManager lastTurn, ToolManager nextTurn)
    {
        if (lastTurn != null)
        {
            A_CharacterSelector manager = playerUiManager.positionToManager[party.GetPosition(lastTurn)];
            manager.GetComponent<UIGradient>().enabled = true;
        }
        if (nextTurn != null)
        {
            A_CharacterSelector manager = playerUiManager.positionToManager[party.GetPosition(nextTurn)];
            manager.GetComponent<UIGradient>().enabled = false;
        }
    }
}

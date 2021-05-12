using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInputState : SingletonScriptableObject<EnemyInputState>, I_GameState
{
    public A_Ability enemyAbility;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
        A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;
        ExecuteInputState executeInputState = ExecuteInputState.Instance;
        
        ToolManager current = enemyParty.GetFirst();

        while (current != null)
        {
            ActionHolder actionHolder = new ActionHolder(enemyAbility, current, enemyParty, playerParty);
            Target target = enemyAbility.GetTargetType();
            I_TargetHolder targetHolder = target.BuildTargetHolder();
            I_Targetable targetable = targetHolder.GetRandomTargetable(enemyParty, playerParty, actionHolder);
            targetHolder.SetTarget(targetable);
            actionHolder.targetHodler = targetHolder;
            executeInputState.AddCombatAction(actionHolder);
            //ActionHolder action = new ActionHolder(enemyAbility, 1f, current);
            //action.Initialize(playerParty.GetRandom());
            //executeInputState.AddCombatAction(action);
            current = enemyParty.GetNext(current);
        }

        yield return null;
        response.nextState = executeInputState;
    }
}

using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInputState : SingletonScriptableObject<EnemyInputState>, I_GameState
{
    public AbilitySO enemyAbility;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
        A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;
        ExecuteInputState executeInputState = ExecuteInputState.Instance;
        
        ToolManager current = enemyParty.GetFirst();

        while (current != null)
        {
            Ability ability = enemyAbility.abilityBuilder.BuildAbility();
            AbilityAction abilityAction = ability.primaryAbilityAction;
            Target target = abilityAction.GetTargetType(current);
            I_TargetHolder targetHolder = target.BuildTargetHolder();
            ActionProcessor actionHolder = new ActionProcessor(abilityAction, current, enemyParty, playerParty, targetHolder);
            targetHolder.GetTargetableByThreat(current, enemyParty, playerParty, actionHolder);
            executeInputState.AddPrimaryAction(actionHolder);
            //ActionHolder action = new ActionHolder(enemyAbility, 1f, current);
            //action.Initialize(playerParty.GetRandom());
            //executeInputState.AddCombatAction(action);
            current = enemyParty.GetNext(current);
        }

        yield return null;
        response.nextState = executeInputState;
    }
}

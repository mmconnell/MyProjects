using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;

public class StartRoundState : SingletonScriptableObject<StartRoundState>, I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
        A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;

        ToolManager member = playerParty.GetFirst();

        foreach (PartyPosition position in playerParty.GetActivePositions())
        {
            TriggerTool triggerTool = playerParty.GetToolManager(position).Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnStart);
        }
        foreach (PartyPosition position in enemyParty.GetActivePositions())
        {
            TriggerTool triggerTool = enemyParty.GetToolManager(position).Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnStart);
        }
        PlayerInputState.Instance.turn += 1;
        BattleLogUIManager.Instance.turnValue.text = PlayerInputState.Instance.turn.ToString();
        CombatProcessorInfo info = new CombatProcessorInfo()
        {
            runner = request.runner,
        };
        yield return ExecuteInputState.Instance.ProcessSupportingActions(info);
        response.nextState = PlayerInputState.Instance;
        yield return new WaitForSeconds(1f);
        yield break;
    }
}

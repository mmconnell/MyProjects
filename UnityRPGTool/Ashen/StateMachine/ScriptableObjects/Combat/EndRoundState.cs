using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;

public class EndRoundState : SingletonScriptableObject<EndRoundState>, I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
        A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;

        ToolManager member = playerParty.GetFirst();

        foreach (PartyPosition position in playerParty.GetActivePositions())
        {
            TriggerTool triggerTool = playerParty.GetToolManager(position).Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnEnd);
        }
        foreach (PartyPosition position in enemyParty.GetActivePositions())
        {
            TriggerTool triggerTool = enemyParty.GetToolManager(position).Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnEnd);
        }

        response.nextState = StartRoundState.Instance;
        yield break;
    }
}

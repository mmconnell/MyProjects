using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatActionExecutor
{
    public void ExecuteActions(List<I_CombatAction> combatActions, BattleFieldState battleFieldState)
    {
        foreach (I_CombatAction combatAction in combatActions)
        {
            if (combatAction.ResolveSelection())
            {

            }
            else
            {

            }
            battleFieldState.ResolveTurn();
            if (!battleFieldState.CanBattleContinue())
            {
                break;
            }
        }
    }
}

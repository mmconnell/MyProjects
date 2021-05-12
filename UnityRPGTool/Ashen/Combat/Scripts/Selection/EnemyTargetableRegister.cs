using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Manager;

public class EnemyTargetableRegister : SerializedMonoBehaviour
{
    public I_Targetable targetable;
    public EnemyPartyManager enemyPartyManager;
    public PartyPosition position;
    public RowHandler rowHandler;

    private void Start()
    {
        if (enemyPartyManager.playerTargetables == null)
        {
            enemyPartyManager.playerTargetables = new I_Targetable[PartyPositions.Count];
        }
        enemyPartyManager.playerTargetables[(int)position] = targetable;
        rowHandler.Recalculate();
    }
}

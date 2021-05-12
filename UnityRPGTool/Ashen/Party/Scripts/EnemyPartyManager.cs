using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;

public class EnemyPartyManager : A_PartyManager
{
    public Dictionary<PartyPosition, EnemyPartyMemberManager> enemyManagers;
    public EnemyPartyUIManager enemyPartyUIManager;

    protected override void Start()
    {
        base.Start();
        EnemyPartyHolder.Instance.enemyPartyManager = this;
        enemyPartyUIManager = EnemyPartyUIManager.Instance;
    }

    public override void SetToolManager(PartyPosition position, ToolManager toolManager)
    {
        base.SetToolManager(position, toolManager);
        playerTargetables[(int)position] = enemyPartyUIManager.positionToManager[position];
    }
}

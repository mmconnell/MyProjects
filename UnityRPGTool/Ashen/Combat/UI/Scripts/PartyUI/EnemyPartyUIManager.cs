using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;

public class EnemyPartyUIManager : A_PartyUIManager<EnemyPartyUIManager>
{ 

    protected override void Start()
    {
        base.Start();
    }

    protected override A_PartyManager GetPartyManager()
    {
        return EnemyPartyHolder.Instance.enemyPartyManager;
    }

    public override void SetPartyMember(PartyPosition position, ToolManager toolManager)
    {
        base.SetPartyMember(position, toolManager);
    }
}

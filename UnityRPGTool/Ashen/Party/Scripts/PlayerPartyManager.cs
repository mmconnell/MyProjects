using UnityEngine;
using System.Collections;
using Manager;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerPartyManager : A_PartyManager
{
    protected override void Start()
    {
        base.Start();
        PlayerPartyHolder.Instance.partyManager = this;
    }
}

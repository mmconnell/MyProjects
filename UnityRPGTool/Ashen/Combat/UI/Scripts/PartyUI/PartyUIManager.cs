using Manager;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyUIManager : A_PartyUIManager<PartyUIManager>
{
    protected override void Start()
    {
        base.Start();
        foreach(PartyPosition position in PartyPositions.Instance)
        {
            if (positionToManager.TryGetValue(position, out A_CharacterSelector manager)) {
                (manager as PartyMemberManager).partyUIManager = this;
            }
        }
        Recalculate();
    }

    // Update is called once per frame
    public override void Recalculate()
    {
        base.Recalculate();
        foreach(A_CharacterSelector manager in managers)
        {
            HandleSlot(manager);
        }
    }

    public void HandleSlot(A_CharacterSelector slot)
    {
        if (!slot.HasRegisteredToolManager())
        {
            slot.gameObject.SetActive(false);
        }
        else
        {
            slot.gameObject.SetActive(true);
        }
    }

    protected override A_PartyManager GetPartyManager()
    {
        return PlayerPartyHolder.Instance.partyManager;
    }
}

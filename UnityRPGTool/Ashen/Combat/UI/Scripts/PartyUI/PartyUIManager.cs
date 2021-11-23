using Manager;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyUIManager : A_PartyUIManager<PartyUIManager>
{
    public GameObject damageTextCanvas;
    public GameObject damageTextPrefab;
    public List<PartyPositionManager> positionManagers;
    public GameObject partyMemberUIPrefab;

    protected override void Start()
    {
        foreach (PartyPositionManager positionManager in positionManagers)
        {
            GameObject partyPosition = Instantiate(partyMemberUIPrefab, positionManager.transform);
            A_CharacterSelector selector = partyPosition.GetComponent<A_CharacterSelector>();

            if (positionToManager == null)
            {
                positionToManager = new Dictionary<PartyPosition, A_CharacterSelector>();
            }

            if (positionToManager.ContainsKey(positionManager.position))
            {
                positionToManager[positionManager.position] = selector;
            }
            else
            {
                positionToManager.Add(positionManager.position, selector);
            }
            
        }
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
            slot.GetDisabler().SetActive(false);
        }
        else
        {
            slot.GetDisabler().SetActive(true);
        }
    }

    protected override A_PartyManager GetPartyManager()
    {
        return PlayerPartyHolder.Instance.partyManager;
    }
}

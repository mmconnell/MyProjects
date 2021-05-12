using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerPartySelector : A_PartySelector
{
    public override void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.chosenTarget = this;
    }
}

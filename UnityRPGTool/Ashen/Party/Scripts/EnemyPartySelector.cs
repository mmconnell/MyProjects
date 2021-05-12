using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EnemyPartySelector : A_PartySelector
{
    public override void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.chosenTarget = this;
    }
}

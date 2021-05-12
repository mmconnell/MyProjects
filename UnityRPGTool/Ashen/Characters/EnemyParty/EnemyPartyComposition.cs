using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartyComposition : SerializedScriptableObject
{
    public Dictionary<PartyPosition, GameObject> partyComposition;
}

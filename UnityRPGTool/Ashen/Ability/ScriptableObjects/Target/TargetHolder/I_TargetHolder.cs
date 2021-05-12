using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;

public interface I_TargetHolder
{
    void ResolveTarget(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder);
    I_Targetable GetTargetable(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder);
    I_Targetable GetRandomTargetable(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder);
    I_TargetHolder Clone();
    List<ToolManager> GetTargets();
    void SetTarget(I_Targetable targetable);
}

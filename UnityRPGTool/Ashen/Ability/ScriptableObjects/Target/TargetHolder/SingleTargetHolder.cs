using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;

public class SingleTargetHolder : I_TargetHolder
{
    public I_Targetable target;

    public I_TargetHolder Clone()
    {
        return new SingleTargetHolder();
    }

    public I_Targetable GetRandomTargetable(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        PartyPosition pos = targetParty.GetRandomPosition();
        I_Targetable targetable = targetParty.playerTargetables[(int)pos];
        return targetable;
    }

    public I_Targetable GetTargetable(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        return targetParty.GetFirstTargetableCharacter();
    }

    public List<ToolManager> GetTargets()
    {
        return this.target.GetTargets();
    }

    public void ResolveTarget(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        throw new System.NotImplementedException();
    }

    public void SetTarget(I_Targetable targetable)
    {
        this.target = targetable;
    }
}

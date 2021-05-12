using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;

public class RowTargetHolder : I_TargetHolder
{
    public I_Targetable target;

    public I_TargetHolder Clone()
    {
        return new RowTargetHolder();
    }

    public I_Targetable GetRandomTargetable(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        return targetParty.GetRandomRow();
    }

    public I_Targetable GetTargetable(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder)
    {
        return targetParty.GetDefaultRowTarget();
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;

public interface I_TargetHolder
{
    I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability);
    bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability);
    void GetRandomTargetable(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder);
    void SetTargetable(ToolManager source, ToolManager target, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionProcessor);
    void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder);
    I_TargetHolder Clone();
    void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState);
    void InitializeTarget(I_Targetable targetable, A_PartyManager sourceParty, A_PartyManager targetParty, PlayerInputState inputState);
    void Cleanup(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState);
    void Initialize();
}

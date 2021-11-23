using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;
using Ashen.DeliverySystem;

public abstract class A_TargetHolder<T> : I_TargetHolder where T : A_TargetHolder<T>, new()
{
    private List<PartyPosition> validPositions;
    private int[] threatMap;

    public virtual I_TargetHolder Clone()
    {
        return new T();
    }

    public void Cleanup(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState)
    {
        if (inputState.currentTarget != null)
        {
            inputState.currentTarget.Deselected();
        }
        if (inputState.nextTarget != null)
        {
            inputState.nextTarget.Deselected();
        }
        inputState.currentTarget = null;
        inputState.nextTarget = null;
        CleanupInternal(sourceParty, targetParty, actionHolder, inputState);
    }

    public void PopulateThreatMap(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        if (threatMap == null)
        {
            threatMap = new int[PartyPositions.Count];
        }
        for (int x = 0; x < threatMap.Length; x++)
        {
            threatMap[x] = 0;
        }
        List<PartyPosition> validPositions = GetValidPositions(source, sourceParty, targetParty, ability);
        foreach (PartyPosition activePosition in targetParty.GetActivePositions())
        {
            if (validPositions.Contains(activePosition))
            {
                ToolManager tm = targetParty.GetToolManager(activePosition);
                AttributeTool at = tm.Get<AttributeTool>();
                threatMap[(int)activePosition] = at.GetAttribute(DerivedAttributes.Instance.threat);
                if (activePosition.row == PartyRow.BACK)
                {
                    threatMap[(int)activePosition] /= 2;
                }
            }
        }
    }

    public PartyPosition GetPositionByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        PopulateThreatMap(source, sourceParty, targetParty, ability);
        List<ThreatPack> threatPacks = new List<ThreatPack>();
        int totalThreat = 0;
        for (int x = 0; x < threatMap.Length; x++)
        {
            if (threatMap[x] > 0)
            {
                totalThreat += threatMap[x];
                ThreatPack pack = new ThreatPack()
                {
                    threat = threatMap[x],
                    position = PartyPositions.Instance[x],
                };
                bool added = false;
                for (int y = 0; y < threatPacks.Count; y++)
                {
                    if (threatPacks[y].threat > pack.threat)
                    {
                        added = true;
                        threatPacks.Insert(y, pack);
                        break;
                    }
                }
                if (!added)
                {
                    threatPacks.Add(pack);
                }
            }
        }
        int threatChoice = Random.Range(0, totalThreat);
        for (int x = 0; x < threatPacks.Count; x++)
        {
            threatChoice -= threatPacks[x].threat;
            if (threatChoice < 0)
            {
                return threatPacks[x].position;
            }
        }
        return null;
    }

    protected virtual void CleanupInternal(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState) { }

    public virtual void InitializeTarget(I_Targetable targetable, A_PartyManager sourceParty, A_PartyManager targetParty, PlayerInputState inputState)
    {
        inputState.nextTarget = targetable;
    }

    public List<PartyPosition> GetValidPositions(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability)
    {
        if (validPositions != null)
        {
            return validPositions;
        }
        validPositions = new List<PartyPosition>();
        validPositions.AddRange(PartyPositions.Instance);
        TargetRange range = ability.GetTargetRange(source);
        PartyPosition sourcePosition = sourceParty.GetPosition(source);
        if (range == TargetRange.EXTENDED_MELEE || range == TargetRange.RANGED || sourceParty == targetParty)
        {
            return validPositions;
        }
        else if (range == TargetRange.MELEE)
        {
            if (sourcePosition.row == PartyRow.FRONT)
            {
                return validPositions;
            }
            foreach (PartyPosition position in PartyPositions.Instance)
            {
                if (position.row == PartyRow.BACK)
                {
                    validPositions.Remove(position);
                }
            }
        }
        return validPositions;
    }

    public virtual void Initialize()
    {
        validPositions = null;
    }

    public virtual void SetTargetable(ToolManager source, ToolManager target, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionProcessor)
    {
        GetRandomTargetable(source, sourceParty, targetParty, actionProcessor);
    }

    public abstract void GetRandomTargetable(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder);
    public abstract I_CombatProcessor ResolveTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability);
    public abstract void ResolveTargetRequest(A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder, PlayerInputState inputState);
    public abstract bool HasNextTarget(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, I_AbilityAction ability);
    public abstract void GetTargetableByThreat(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, ActionProcessor actionHolder);
}

struct ThreatPack
{
    public PartyPosition position;
    public int threat;
}

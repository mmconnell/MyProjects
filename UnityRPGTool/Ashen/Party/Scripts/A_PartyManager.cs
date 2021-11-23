using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;

public abstract class A_PartyManager : SerializedMonoBehaviour
{
    [ShowInInspector]
    protected ToolManager[] toolManagerPosition;

    public Dictionary<PartyPosition, ToolManager> defaultPositions;
    public HashSet<PartyPosition> enabledPositions;

    [NonSerialized]
    public I_Targetable[] playerTargetables;

    private void Awake()
    {
        if (playerTargetables == null || playerTargetables.Length != PartyPositions.Count)
        {
            playerTargetables = new I_Targetable[PartyPositions.Count];
        }
        toolManagerPosition = new ToolManager[PartyPositions.Count];
    }

    protected virtual void Start()
    {
        if (defaultPositions != null)
        {
            foreach(PartyPosition position in PartyPositions.Instance)
            {
                if (defaultPositions.TryGetValue(position, out ToolManager toolManager))
                {
                    SetToolManager(position, toolManager);
                }
            }
        }
    }

    public ToolManager GetFirst()
    {
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)partyPosition] != null)
            {
                return toolManagerPosition[(int)partyPosition];
            }
        }
        return null;
    }

    public I_Targetable GetFirstTargetableCharacter()
    {
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)partyPosition] != null)
            {
                return playerTargetables[(int)partyPosition];
            }
        }
        return null;
    }

    public I_Targetable GetFirstTargetableCharacterInRow(PartyRow partyRow)
    {
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (partyPosition.row == partyRow && toolManagerPosition[(int)partyPosition] != null)
            {
                return playerTargetables[(int)partyPosition];
            }
        }
        return null;
    }

    public I_Targetable GetLastTargetableCharacterInRow(PartyRow partyRow)
    {
        I_Targetable last = null;
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (partyPosition.row == partyRow && toolManagerPosition[(int)partyPosition] != null)
            {
                last = playerTargetables[(int)partyPosition];
            }
        }
        return last;
    }

    public PartyPosition GetNextTargetableCharacterInRow(PartyPosition position)
    {
        bool found = false;
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (partyPosition == position)
            {
                found = true;
                continue;
            }
            if (!found)
            {
                continue;
            }
            if (partyPosition.row == position.row && toolManagerPosition[(int)partyPosition] != null)
            {
                return partyPosition;
            }
        }
        return null;
    }

    public PartyPosition GetPreviousTargetableCharacterInRow(PartyPosition position)
    {
        PartyPosition previous = null;
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (partyPosition == position)
            {
                break;
            }
            if (partyPosition.row == position.row && toolManagerPosition[(int)partyPosition] != null)
            {
                previous = partyPosition;
            }
        }
        return previous;
    }

    public List<ToolManager> GetRowTargets(PartyRow partyRow)
    {
        List<ToolManager> toolManagers = new List<ToolManager>();
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (partyPosition.row == partyRow && toolManagerPosition[(int)partyPosition] != null)
            {
                toolManagers.Add(toolManagerPosition[(int)partyPosition]);
            }
        }
        return toolManagers;
    }

    public List<ToolManager> GetAll()
    {
        List<ToolManager> toolManagers = new List<ToolManager>();
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)partyPosition] != null)
            {
                toolManagers.Add(toolManagerPosition[(int)partyPosition]);
            }
        }
        return toolManagers;
    }

    public ToolManager GetNext(ToolManager toolManager)
    {
        PartyPosition position = GetPosition(toolManager);
        for (int x = (int)position + 1; x < PartyPositions.Count; x++)
        {
            if (toolManagerPosition[x] != null)
            {
                return toolManagerPosition[x];
            }
        }
        return null;
    }

    public ToolManager GetPrevious(ToolManager toolManager)
    {
        PartyPosition position = GetPosition(toolManager);
        for (int x = (int)position - 1; x >= 0; x--)
        {
            if (toolManagerPosition[x] != null)
            {
                return toolManagerPosition[x];
            }
        }
        return null;
    }

    public void StartSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)partyPosition] != null)
            {
                EventSystem.current.SetSelectedGameObject(toolManagerPosition[(int)partyPosition].gameObject);
                break;
            }
        }
    }

    public PartyPosition GetRandomPosition()
    {
        List<int> indexes = new List<int>();
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)position] != null)
            {
                indexes.Add((int)position);
            }
        }

        int random = UnityEngine.Random.Range(0, indexes.Count);
        return PartyPositions.Instance[indexes[random]];
    }

    public PartyRow GetRandomRow()
    {
        List<PartyRow> rows = new List<PartyRow>();
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)position] != null && !rows.Contains(position.row))
            {
                rows.Add(position.row);
            }
        }
        int random = UnityEngine.Random.Range(0, rows.Count);
        return rows[random];
    }

    public ToolManager GetRandom()
    {
        List<int> indexes = new List<int>();
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)position] != null)
            {
                indexes.Add((int)position);
            }
        }
        
        int random = UnityEngine.Random.Range(0, indexes.Count);
        return toolManagerPosition[indexes[random]];
    }

    public ToolManager GetRandom(List<PartyPosition> validPositions)
    {
        List<int> indexes = new List<int>();
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (validPositions.Contains(position) && toolManagerPosition[(int)position] != null)
            {
                indexes.Add((int)position);
            }
        }

        int random = UnityEngine.Random.Range(0, indexes.Count);
        return toolManagerPosition[indexes[random]];
    }

    public PartyPosition GetRandomInRow(PartyRow row)
    {
        List<PartyPosition> positions = new List<PartyPosition>();
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)position] != null && position.row == row)
            {
                positions.Add(position);
            }
        }

        int random = UnityEngine.Random.Range(0, positions.Count);
        return positions[random];
    }

    public I_Targetable GetDefaultSingleTarget()
    {
        for (int x = 0; x < PartyPositions.Count; x++)
        {
            if (toolManagerPosition[x] != null)
            {
                return playerTargetables[x];
            }
        }
        return null;
    }

    public virtual void SetToolManager(PartyPosition position, ToolManager toolManager)
    {
        toolManagerPosition[(int)position] = toolManager;
    }

    public ToolManager GetToolManager(PartyPosition position)
    {
        return toolManagerPosition[(int)position];
    }

    public ToolManager GetToolManager(int position)
    {
        return toolManagerPosition[position];
    }

    public I_Targetable GetTargetable(PartyPosition position)
    {
        return playerTargetables[(int)position];
    }

    public void DeselectAll()
    {
        foreach (I_Targetable targetable in playerTargetables)
        {
            if (targetable != null)
            {
                targetable.Deselected();
            }
        }
    }

    public void Swap(PartyPosition first, PartyPosition second)
    {
        ToolManager temp = toolManagerPosition[(int)first];
        SetToolManager(first, toolManagerPosition[(int)second]);
        toolManagerPosition[(int)second] = temp;
    }

    public void SwapRows()
    {
        PartyPositions partyPositions = PartyPositions.Instance;
        for (int x = 0; x < partyPositions.frontRow.Count; x++)
        { 
            if (enabledPositions == null || enabledPositions.Count == 0 || enabledPositions.Contains(partyPositions.frontRow[x]))
            {
                ToolManager temp = toolManagerPosition[(int)partyPositions.frontRow[x]];
                toolManagerPosition[(int)partyPositions.frontRow[x]] = toolManagerPosition[(int)partyPositions.backRow[x]];
                toolManagerPosition[(int)partyPositions.backRow[x]] = temp;
            }
        }
    }

    public PartyPosition GetPosition(ToolManager toolManager)
    {
        if (toolManager == null)
        {
            return null;
        }
        for (int x = 0; x < toolManagerPosition.Length; x++)
        {
            if (toolManagerPosition[x] == null)
            {
                continue;
            }
            if (ReferenceEquals(toolManager.gameObject, toolManagerPosition[x].gameObject))
            {
                return PartyPositions.Instance[x];
            }
        }
        return null;
    }

    public bool HasActivePositionsInRow(PartyRow row)
    {
        for (int x = 0; x < PartyPositions.Count; x++)
        {
            if (toolManagerPosition[x] != null && PartyPositions.Instance[x].row == row)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerable<PartyPosition> GetActivePositionsInRow(PartyRow row)
    {
        for (int x = 0; x < PartyPositions.Count; x++)
        {
            if (toolManagerPosition[x] != null && PartyPositions.Instance[x].row == row)
            {
                yield return PartyPositions.Instance[x];
            }
        }
    }

    public IEnumerable<PartyPosition> GetActivePositions()
    {
        for (int x = 0; x < PartyPositions.Count; x++)
        {
            if (toolManagerPosition[x] != null)
            {
                yield return PartyPositions.Instance[x];
            }
        }
    }
}

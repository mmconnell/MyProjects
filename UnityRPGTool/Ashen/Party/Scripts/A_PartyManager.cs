using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;

public abstract class A_PartyManager : SerializedMonoBehaviour
{
    protected ToolManager[] toolManagerPosition;

    public Dictionary<PartyPosition, ToolManager> defaultPositions;
    public HashSet<PartyPosition> enabledPositions;

    public List<RowHandler> rowTargetables;
    public I_Targetable defaultRowTargetable;
    public I_Targetable partyTargetable;

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
        return PartyPositions.Instance[random];
    }

    public RowHandler GetRandomRow()
    {
        bool[] enabledRows = new bool[Enum.GetValues(typeof(PartyRow)).Length];
        List<int> indexes = new List<int>();
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)position] != null)
            {
                enabledRows[(int)position.row] = true;
            }
        }
        for (int x = 0; x < rowTargetables.Count; x++)
        {
            if (enabledRows[(int)rowTargetables[x].row])
            {
                indexes.Add(x);
            }
        }
        int random = UnityEngine.Random.Range(0, indexes.Count);
        return rowTargetables[random];
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

    public I_Targetable GetDefaultRowTarget()
    {
        if (defaultRowTargetable == null)
        {
            defaultRowTargetable = rowTargetables[0];
        }
        return defaultRowTargetable;
    }

    public I_Targetable GetPartyTarget()
    {
        return partyTargetable;
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
}

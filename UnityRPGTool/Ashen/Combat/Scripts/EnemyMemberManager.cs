using UnityEngine;
using System.Collections;
using Manager;

public class EnemyMemberManager : MonoBehaviour
{
    private ToolManager toolManager;
    public RowHandler rowHandler;

    public void RegisterToolManager(ToolManager toolManager)
    {
        UnregisterToolManager();
        this.toolManager = toolManager;
        if (toolManager)
        {

        }
        rowHandler.Recalculate();
    }

    public void UnregisterToolManager()
    {
        if (toolManager)
        {

        }
        this.toolManager = null;
        rowHandler.Recalculate();
    }

    public bool HasRegisteredToolManager()
    {
        return this.toolManager;
    }
}

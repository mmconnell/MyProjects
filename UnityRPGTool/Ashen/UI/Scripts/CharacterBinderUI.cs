using UnityEngine;
using System.Collections;
using Manager;

public class CharacterBinderUI : MonoBehaviour
{
    public static ToolManager pressedStart;
    public ToolManager boundTool;
    
    void Awake()
    {
        if (pressedStart == null)
        {
            Logger.DebugLog("No one pressed start");
        }
        else
        {
            boundTool = pressedStart;
        }
    }
}

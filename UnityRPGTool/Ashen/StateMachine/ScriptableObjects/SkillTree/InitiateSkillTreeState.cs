using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InitiateSkillTreeState : SingletonScriptableObject<InitiateSkillTreeState>, I_GameState
{
    public string uiSceneName;

    [NonSerialized]
    public ToolManager currentManager;
    [NonSerialized]
    public List<ToolManager> allToolManagers;
    [NonSerialized]
    public int curManager;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        currentManager = null;
        allToolManagers = new List<ToolManager>();

        AsyncOperation operation = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            Debug.Log("Loading ui Scene: " + uiSceneName + " Progress: " + operation.progress);
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByName(uiSceneName);

        yield return null;
        
        A_PartyManager partyManager = PlayerPartyHolder.Instance.partyManager;
        SkillTreeUI skillTreeUi = SkillTreeUI.Instance;
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            ToolManager toolManager = partyManager.GetToolManager(position);
            if (toolManager && !currentManager)
            {
                currentManager = toolManager;
                curManager = 0;
                SkillTreeTool skillTreeTool = currentManager.Get<SkillTreeTool>();
                skillTreeUi.RegisterSkillTree(skillTreeTool);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(skillTreeUi.skillTreeUIs[0].gameObject);
            }
            if (toolManager)
            {
                allToolManagers.Add(toolManager);
            }
        }

        yield return null;

        while (true)
        {
            int lastIndex = curManager;
            if (Input.GetKeyDown("joystick 1 button 4"))
            {
                curManager -= 1;
                if (curManager < 0)
                {
                    curManager = allToolManagers.Count - 1;
                }
            }
            else if (Input.GetKeyDown("joystick 1 button 5"))
            {
                curManager += 1;
                if (curManager >= allToolManagers.Count)
                {
                    curManager = 0;
                }
            }
            else if (Input.GetKeyDown("joystick 1 button 6"))
            {
                EventSystem.current.SetSelectedGameObject(null);

                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(uiSceneName);

                while (!unloadOperation.isDone)
                {
                    Debug.Log("Unloading ui Scene: " + uiSceneName + " Progress: " + unloadOperation.progress);
                    yield return null;
                }

                yield break;
            }

            if (lastIndex != curManager)
            {
                currentManager = allToolManagers[curManager];
                SkillTreeTool skillTreeTool = currentManager.Get<SkillTreeTool>();
                skillTreeUi.RegisterSkillTree(skillTreeTool);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(skillTreeUi.skillTreeUIs[0].gameObject);
            }

            yield return null;
        }
    }
}

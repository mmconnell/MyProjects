using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitiateCombatState : SingletonScriptableObject<InitiateCombatState>, I_GameState
{
    public string uiSceneName;
    public EnemyPartyComposition enemyPartyComp;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        ToolManager[] managers = GameObject.FindObjectsOfType<ToolManager>();
        AsyncOperation operation = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            Debug.Log("Loading ui Scene: " + uiSceneName + " Progress: " + operation.progress);
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByName(uiSceneName);
        ActionOptionsManager.Instance.gameObject.SetActive(false);

        yield return null;

        PartyUIManager partyUi = PartyUIManager.Instance;
        A_PartyManager partyManager = PlayerPartyHolder.Instance.partyManager;
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            partyUi.SetPartyMember(position, partyManager.GetToolManager(position));
        }

        EnemyPartyManager enemyPartyManager = EnemyPartyHolder.Instance.enemyPartyManager;
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (enemyPartyComp.partyComposition.TryGetValue(position, out GameObject prefab))
            {
                GameObject enemy = Instantiate(prefab, enemyPartyManager.enemyManagers[position].transform);
                ToolManager enemyTool = enemy.GetComponent<ToolManager>();
                EnemyPartyUIManager.Instance.SetPartyMember(position, enemyTool);
                enemyPartyManager.SetToolManager(position, enemyTool);
            }
        }
        //GameObject go = Instantiate(testEnemy);
        //go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 1.2f, go.transform.position.z);

        yield return new WaitForSeconds(1f);
        ExecuteInputState.Instance.Reset();
        response.nextState = PlayerInputState.Instance;
    }
}

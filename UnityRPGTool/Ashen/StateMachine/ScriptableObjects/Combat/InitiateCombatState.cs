using Ashen.DeliverySystem;
using Manager;
using System.Collections;
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
        PlayerInputState.Instance.turn = 0;
        BattleLogUIManager.Instance.turnValue.text = "1";
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            ToolManager toolManager = partyManager.GetToolManager(position);
            partyUi.SetPartyMember(position, partyManager.GetToolManager(position));
            if (toolManager)
            {
                partyManager.playerTargetables[(int)position] = partyUi.positionToManager[position];
                TriggerTool triggerTool = toolManager.Get<TriggerTool>();
                triggerTool.Trigger(ExtendedEffectTriggers.Instance.BattleStart);
            }
        }

        EnemyPartyManager enemyPartyManager = EnemyPartyHolder.Instance.enemyPartyManager;
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (enemyPartyComp.partyComposition.TryGetValue(position, out GameObject prefab))
            {
                GameObject enemy = Instantiate(prefab, enemyPartyManager.enemyManagers[position].transform);
                ToolManager enemyTool = enemy.GetComponent<ToolManager>();
                TriggerTool triggerTool = enemyTool.Get<TriggerTool>();
                triggerTool.Trigger(ExtendedEffectTriggers.Instance.BattleStart);
                EnemyPartyUIManager.Instance.SetPartyMember(position, enemyTool);
                enemyPartyManager.SetToolManager(position, enemyTool);
            }
        }
        //GameObject go = Instantiate(testEnemy);
        //go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 1.2f, go.transform.position.z);

        yield return new WaitForSeconds(1f);
        ExecuteInputState.Instance.Reset();
        PlayerInputState.Instance.Initialize();
        response.nextState = StartRoundState.Instance;
    }
}

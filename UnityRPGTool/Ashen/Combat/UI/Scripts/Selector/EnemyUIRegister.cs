using UnityEngine;
using System.Collections;
using Manager;

public class EnemyUIRegister : MonoBehaviour
{
    public ResourceBarManager healthBar;
    public ToolManager toolManager;

    public void Start()
    {
        healthBar.RegisterToolManager(toolManager, ResourceValues.Instance.health);
    }

    public void OnDestroy()
    {
        if (healthBar)
        {
            healthBar.UnregisterToolManager();
        }
    }
}

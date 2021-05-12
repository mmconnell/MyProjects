using UnityEngine;
using System.Collections;
using Manager;
using UnityEngine.EventSystems;

public class SkillOptionExecutor : MonoBehaviour, I_OptionExecutor
{
    public SkillPanelHandler skillPanel;
    public CombatOptionUI combatOption;

    public void ExecuteOption(ToolManager source)
    {
        SkillSelector skillSelector = skillPanel.GetFirstSkill();
        if (skillSelector != null)
        {
            skillPanel.enabler.SetActive(true);
            EventSystemHelper.Instance.UpdateSelected(skillSelector.gameObject);
        }
    }

    public void Cancel()
    {
        skillPanel.enabler.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void TurnOff()
    {
        skillPanel.enabler.SetActive(false);
    }
}

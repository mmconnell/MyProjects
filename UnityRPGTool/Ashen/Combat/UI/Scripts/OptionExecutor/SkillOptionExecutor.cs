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
        AbilityHolder abilityHolder = source.Get<AbilityHolder>();
        if (abilityHolder.GetCount() == 0)
        {
            Cancel();
            return;
        }
        skillPanel.enabler.SetActive(true);
        skillPanel.LoadSkills(abilityHolder);
        EventSystemHelper.Instance.UpdateSelected(skillSelector.gameObject);
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

    public void InitializeOption(ToolManager source)
    {
        AbilityHolder abilityHolder = source.Get<AbilityHolder>();
        if (abilityHolder)
        {
            if (abilityHolder.GetCount() > 0)
            {
                combatOption.Valid = true;
            }
            else
            {
                combatOption.Valid = false;
            }
        }
        else
        {
            combatOption.Valid = false;
        }
    }
}

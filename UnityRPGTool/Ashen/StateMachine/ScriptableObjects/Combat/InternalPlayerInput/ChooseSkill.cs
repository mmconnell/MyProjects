using UnityEngine;
using System.Collections;
using Manager;

public class ChooseSkill : I_GameState
{
    private I_GameState previousState;
    private Ability previousAbility;
    private SkillSelector previousHoverSkillSelector;

    private I_GameState lastTurnNextState;

    public ChooseSkill(I_GameState previousState)
    {
        this.previousState = previousState;
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        previousHoverSkillSelector = null;
        PlayerInputState inputState = PlayerInputState.Instance;
        AbilityHolder abilityHolder = inputState.currentlySelected.Get<AbilityHolder>();
        if (abilityHolder.GetCount() == 0)
        {
            response.nextState = previousState;
            yield break; ;
        }
        SkillPanelHandler skillPanel = SkillPanelHandler.Instance;
        skillPanel.enabler.SetActive(true);
        skillPanel.LoadSkills(abilityHolder);
        SkillSelector selector = null;
        if (previousAbility != null)
        {
            selector = skillPanel.GetSkillForAbility(previousAbility);
        }
        if (selector == null)
        {
            selector = skillPanel.GetFirstSkill();
        }
        
        EventSystemHelper.Instance.UpdateSelected(selector.gameObject);
        inputState.hoveredSkillSelector = selector;

        while (true)
        {
            if (inputState.hoveredSkillSelector)
            {
                if (previousHoverSkillSelector != null && previousHoverSkillSelector != inputState.hoveredSkillSelector)
                {
                    previousHoverSkillSelector.GradientEnabled(false);
                }
                skillPanel.UpdateSelection(inputState.hoveredSkillSelector);
                inputState.hoveredSkillSelector.GradientEnabled(true);
                previousHoverSkillSelector = inputState.hoveredSkillSelector;
                inputState.hoveredSkillSelector = null;
            }
            if (inputState.backRequested)
            {
                if (previousHoverSkillSelector != null)
                {
                    previousHoverSkillSelector.GradientEnabled(false);
                }
                inputState.backRequested = false;
                response.nextState = previousState;
                skillPanel.enabler.SetActive(false);
                yield break;
            }
            if (inputState.submittedSKillSelector)
            {
                response.nextState = new ChooseTarget(this, inputState.submittedSKillSelector.ability);
                lastTurnNextState = response.nextState;
                previousAbility = inputState.submittedSKillSelector.ability;
                inputState.submittedSKillSelector.GradientEnabled(false);
                inputState.submittedSKillSelector = null;
                skillPanel.enabler.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
}

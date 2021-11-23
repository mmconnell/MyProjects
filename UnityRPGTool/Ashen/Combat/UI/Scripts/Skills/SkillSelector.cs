using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JoshH.UI;
using TMPro;

public class SkillSelector : Selectable, ISubmitHandler, ICancelHandler
{
    public UIGradient gradient;
    public Ability ability;
    public SkillOptionExecutor skillOptionExecutor;
    public SkillPanelHandler skillPanel;
    public TextMeshProUGUI skillCost;
    public TextMeshProUGUI skillName;
    public Image background;

    private bool valid;
    public bool Valid
    {
        set
        {
            if (value)
            {
                background.color = skillPanel.validOption.background;
                gradient.LinearColor1 = skillPanel.validOption.color1;
                gradient.LinearColor2 = skillPanel.validOption.color2;
                skillName.color = skillPanel.validOption.name;
                skillCost.color = skillPanel.validOption.cost;
            }
            else
            {
                background.color = skillPanel.invalidOption.background;
                gradient.LinearColor1 = skillPanel.invalidOption.color1;
                gradient.LinearColor2 = skillPanel.invalidOption.color2;
                skillName.color = skillPanel.invalidOption.name;
                skillCost.color = skillPanel.invalidOption.cost;
            }
            valid = value;
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (valid)
        {
            PlayerInputState.Instance.submittedSKillSelector = this;
        }
    }

    protected override void Start()
    {
        base.Start();
        gradient.enabled = false;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        PlayerInputState.Instance.hoveredSkillSelector = this;
    }

    public override void OnDeselect(BaseEventData eventData)
    {}

    public void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }

    public void GradientEnabled(bool enabled)
    {
        gradient.enabled = enabled;
    }
}

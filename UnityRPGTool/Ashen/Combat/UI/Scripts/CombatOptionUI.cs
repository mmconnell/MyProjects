using DG.Tweening;
using JoshH.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatOptionUI : Selectable, ISubmitHandler, ICancelHandler
{
    public GameObject emptySpace;
    [SerializeField]
    private UIGradient gradient;
    public TweenLayoutElement layoutElement;
    public CombatOption combatOption;
    public Image background;
    public Image border;
    public TextMeshProUGUI title;

    private string iconTweenId;

    private bool valid;
    public bool Valid
    {
        set
        {
            ActionOptionsManager manager = ActionOptionsManager.Instance;
            if (value)
            {
                border.color = manager.validOption.border;
                background.color = manager.validOption.background;
                title.color = manager.validOption.title;
                gradient.LinearColor1 = manager.validOption.color1;
                gradient.LinearColor2 = manager.validOption.color2;
            }
            else
            {
                border.color = manager.invalidOption.border;
                background.color = manager.invalidOption.background;
                title.color = manager.invalidOption.title;
                gradient.LinearColor1 = manager.invalidOption.color1;
                gradient.LinearColor2 = manager.invalidOption.color2;
            }
            valid = value;
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.submittedCombatOption = combatOption;
    }

    public void Restart()
    {
        gradient.enabled = false;
    }

    protected override void Start()
    {
        base.Start();
        gradient.enabled = false;
        iconTweenId = gameObject.name + "Icon";
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (valid)
        {
            DOTween.Restart(iconTweenId, false);
            DOTween.Play(iconTweenId);
        }
        layoutElement.Play();
        ActionOptionsManager.Instance.currentlySelected = this;
        PlayerInputState.Instance.hoveredCombatOption = combatOption;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (valid)
        {
            DOTween.Complete(iconTweenId);
            DOTween.PlayBackwards(iconTweenId);
        }
        layoutElement.Rewind();
    }

    public void GradientEnabled(bool enabled)
    {
        gradient.enabled = enabled;
    }

    public void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }
}

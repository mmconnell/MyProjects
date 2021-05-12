using DG.Tweening;
using JoshH.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatOptionUI : Selectable, ISubmitHandler, ICancelHandler
{
    public GameObject emptySpace;
    public UIGradient gradient;
    private string iconTweenId;
    public TweenLayoutElement layoutElement;
    private bool maintain = false;

    public void OnSubmit(BaseEventData eventData)
    {
        maintain = true;
        GetComponent<I_OptionExecutor>().ExecuteOption(PlayerInputState.Instance.currentlySelected);
    }

    public void Restart()
    {
        maintain = false;
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
        gradient.enabled = true;
        DOTween.Restart(iconTweenId, false);
        layoutElement.Play();
        DOTween.Play(iconTweenId);
        ActionOptionsManager.Instance.currentlySelected = this;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (!maintain)
        {
            gradient.enabled = false;
        }
        DOTween.Complete(iconTweenId);
        layoutElement.Rewind();
        DOTween.PlayBackwards(iconTweenId);
    }

    public void OnCancel(BaseEventData eventData)
    {
        PlayerInputState.Instance.backRequested = true;
    }
}

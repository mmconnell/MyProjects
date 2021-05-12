using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JoshH.UI;

public class SkillSelector : Selectable, ISubmitHandler, ICancelHandler
{
    public UIGradient gradient;
    public A_Ability ability;
    public SkillOptionExecutor skillOptionExecutor;

    public void Initialize(SkillOptionExecutor executor, A_Ability ability)
    {
        this.skillOptionExecutor = executor;
        this.ability = ability;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayerInputState.Instance.chosenAbility = ability;
        skillOptionExecutor.TurnOff();
    }

    protected override void Start()
    {
        base.Start();
        gradient.enabled = false;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        gradient.enabled = true;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        gradient.enabled = false;
    }

    public void OnCancel(BaseEventData eventData)
    {
        skillOptionExecutor.Cancel();
    }
}

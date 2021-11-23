using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;
using Ashen.SkillTree;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using TMPro;

public class SkillTreeNodeUI : Selectable, ISubmitHandler
{
    public SkillTreeUI skillTreeUI;
    public SkillNode skillNode;

    [FoldoutGroup("Elements")] public Image background;
    [FoldoutGroup("Elements")] public Image textBackground;
    [FoldoutGroup("Elements")] public Image currentBackground;
    [FoldoutGroup("Elements")] public Image totalImage;
    [FoldoutGroup("Elements")] public TextMeshProUGUI skillNameText;
    [FoldoutGroup("Elements")] public TextMeshProUGUI currentValueText;
    [FoldoutGroup("Elements")] public TextMeshProUGUI maxValueText;

    public UnityEvent onFirstSKillPoint;
    public UnityEvent onResetToZero;

    public SkillLevel skillLevel;

    public bool valid = false;
    public bool selected = false;

    EventSystem evt;
    EventSystem Evt
    {
        get
        {
            if (evt == null)
            {
                evt = EventSystem.current;
            }
            return evt;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            skillTreeUI.OnClickNode(this, skillNode);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            skillTreeUI.OnClickRight(this, skillNode);
        }

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Evt.SetSelectedGameObject(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnSubmit(BaseEventData eventData)
    {
        skillTreeUI.OnClickNode(this, skillNode);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        selected = true;
        background.color = skillTreeUI.selectedNodeColor;
        if (valid)
        {
            OnValidOptionSkill();
        }
        else
        {
            OnDisabledSkill();
        }
        skillTreeUI.UpdateScreenPosition(skillLevel);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        selected = false;
        if (valid)
        {
            OnValidOptionSkill();
        }
        else
        {
            OnDisabledSkill();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        skillTreeUI.OnClickNode(this, skillNode);
    }

    public void OnDisabledSkill()
    {
        valid = false;
        if (selected)
        {
            background.color = skillTreeUI.selectedNodeColor;
        }
        else
        {
            background.color = skillTreeUI.invalidNode.one;
        }
        textBackground.color = skillTreeUI.invalidNode.two;
        currentBackground.color = skillTreeUI.invalidNode.one;
        totalImage.color = skillTreeUI.invalidNode.three;
        skillNameText.color = skillTreeUI.invalidNode.one;
        currentValueText.color = skillTreeUI.invalidNode.two;
        maxValueText.color = skillTreeUI.invalidNode.one;
    }

    public void OnMaxSkill()
    {
        OnValidOptionSkill();
    }

    public void OnValidOptionSkill()
    {
        valid = true;
        if (selected)
        {
            background.color = skillTreeUI.selectedNodeColor;
        }
        else
        {
            background.color = skillTreeUI.validNode.one;
        }
        textBackground.color = skillTreeUI.validNode.two;
        currentBackground.color = skillTreeUI.validNode.one;
        totalImage.color = skillTreeUI.validNode.three;
        skillNameText.color = skillTreeUI.validNode.one;
        currentValueText.color = skillTreeUI.validNode.two;
        maxValueText.color = skillTreeUI.validNode.one;
    }

    public void OnMissingSkillPoints()
    {
        OnValidOptionSkill();
    }

    public void OnResetToZero()
    {
        if (onResetToZero != null)
        {
            onResetToZero.Invoke();
        }
    }

    public void OnFirstSkillPoint()
    {
        if (onFirstSKillPoint != null)
        {
            onFirstSKillPoint.Invoke();
        }
    }

    public void RegisterSkillNode(SkillNode skillNode)
    {
        this.skillNode = skillNode;
        skillNameText.text = skillNode.skillName;
        maxValueText.text = skillNode.maxRanks.ToString();
        currentValueText.text = "0";
    }
}

public enum SkillLevel
{
    Novice, Veteran, Master
}
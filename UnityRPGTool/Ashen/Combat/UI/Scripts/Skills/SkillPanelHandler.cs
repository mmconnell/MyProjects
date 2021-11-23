using UnityEngine;
using System.Collections;
using Manager;
using System.Collections.Generic;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;

public class SkillPanelHandler : SingletonMonoBehaviour<SkillPanelHandler>
{
    public GameObject skillsHolder;
    public GameObject enabler;
    public GameObject skillPrefab;

    [Hide, FoldoutGroup("Color Scheme"), Title("Valid")]
    public SkillColorTheme validOption;
    [Hide, FoldoutGroup("Color Scheme"), Title("Invalid")]
    public SkillColorTheme invalidOption;

    public ScrollRect scrollRect;
    public RectTransform skillPanelRect;
    public RectTransform fullContentRect;
    public int numberOfSkills;
    public VerticalLayoutGroup skillVerticalLayoutGroup;

    private int activeSkills = 0;
    private List<SkillSelector> skills;

    private float height;
    private int windowStart;

    public float initialOffset;
    public float offsetPerSkill;

    private void Start()
    {
        skills = new List<SkillSelector>();
        RectTransform skillRect = skillPrefab.GetComponent<RectTransform>();
        height = skillRect.rect.height;
    }

    public void LoadSkills(AbilityHolder abilityHolder)
    {
        ResourceValueTool rvTool = abilityHolder.toolManager.Get<ResourceValueTool>();
        DeliveryTool dTool = abilityHolder.toolManager.Get<DeliveryTool>();
        int x = 0;
        SkillSelector lastSkill = null;
        SkillSelector firstSkill = null;
        int totalSizeCount = Mathf.Max(abilityHolder.GetCount(), numberOfSkills);
        skillPanelRect.sizeDelta = new Vector2(skillPanelRect.sizeDelta.x, (height * numberOfSkills) + (offsetPerSkill * (numberOfSkills)) + initialOffset);
        fullContentRect.sizeDelta = new Vector2(fullContentRect.sizeDelta.x, (height * totalSizeCount) + ((totalSizeCount - 1) * offsetPerSkill) + initialOffset + initialOffset);
        foreach (Ability ability in abilityHolder.GetAbilities())
        {
            SkillSelector skillSelector = null;
            if (skills.Count > x)
            {
                skillSelector = skills[x];
            }
            else
            {
                GameObject skill = Instantiate(skillPrefab, skillsHolder.transform);
                skillSelector = skill.GetComponent<SkillSelector>();
                skillSelector.skillPanel = this;
                skills.Add(skillSelector);
                RectTransform skillRect = skillSelector.transform as RectTransform;
                skillSelector.skillName.rectTransform.sizeDelta = new Vector2(skillRect.rect.width / 2f, skillSelector.skillName.rectTransform.sizeDelta.y);
                skillSelector.skillCost.rectTransform.sizeDelta = new Vector2(skillRect.rect.width / 2f, skillSelector.skillCost.rectTransform.sizeDelta.y);
                skillSelector.transform.localPosition = new Vector3(skillSelector.transform.localPosition.x, -(initialOffset + ((skillRect.rect.height + offsetPerSkill) * x)));
            }
            if (firstSkill == null)
            {
                firstSkill = skillSelector;
            }
            skillSelector.ability = ability;
            skillSelector.skillName.text = ability.name;
            I_Equation resourceCost = ability.primaryAbilityAction.primaryResourceCost;
            I_Equation resourceGeneration = ability.primaryAbilityAction.primaryResourceGenerator;
            int totalCost = ((resourceCost == null ? 0 : (int)resourceCost.Calculate(dTool)) - (resourceGeneration == null ? 0 : (int)resourceGeneration.Calculate(dTool)));
            if (totalCost == 0)
            {
                skillSelector.skillCost.text = "";
            }
            else if (totalCost < 0)
            {
                skillSelector.skillCost.text = "^" + (-totalCost);
            }
            else
            {
                skillSelector.skillCost.text = totalCost.ToString();
            }
            skillSelector.Valid = abilityHolder.ValidAbility(ability);
            skillSelector.gameObject.SetActive(true);
            if (lastSkill != null)
            {
                Navigation curNav = skillSelector.navigation;
                curNav.selectOnUp = lastSkill;
                skillSelector.navigation = curNav;
                Navigation lastNav = lastSkill.navigation;
                lastNav.selectOnDown = skillSelector;
                lastSkill.navigation = lastNav;
            }
            lastSkill = skillSelector;
            x++;
        }
        activeSkills = x;
        if (lastSkill != null && firstSkill != null) 
        {
            Navigation last = lastSkill.navigation;
            Navigation first = firstSkill.navigation;
            last.selectOnDown = firstSkill;
            first.selectOnUp = lastSkill;
            lastSkill.navigation = last;
            firstSkill.navigation = first;
        }
        for (; x < skills.Count; x++)
        {
            skills[x].gameObject.SetActive(false);
        }
    }

    public SkillSelector GetSkillForAbility(Ability ability)
    {
        foreach (SkillSelector selector in skills)
        {
            if (selector.ability == ability)
            {
                return selector;
            }
        }
        return null;
    }

    public SkillSelector GetFirstSkill()
    {
        foreach (Transform child in skillsHolder.transform)
        {
            SkillSelector skillSelector = child.GetComponent<SkillSelector>();
            if (skillSelector != null)
            {
                return skillSelector;
            }
        }
        return null;
    }

    public void UpdateSelection(SkillSelector skillSelector)
    {
        int index = skills.IndexOf(skillSelector);
        if (index >= windowStart && index < windowStart + numberOfSkills)
        {
            return;
        }
        if (index == 0)
        {
            windowStart = 0;
        }
        else if (index == skills.Count - 1)
        {
            windowStart = index - numberOfSkills + 1;
        }
        else if (index < windowStart)
        {
            windowStart--;
        }
        else {
            windowStart++;
        }
        int total = windowStart;
        float scrollHeight = (height * total) + (offsetPerSkill * total);
        float percentage = 1 - (scrollHeight / (fullContentRect.sizeDelta.y - (numberOfSkills * height) - (numberOfSkills * offsetPerSkill) - initialOffset));
        scrollRect.verticalNormalizedPosition = percentage;
    }
}

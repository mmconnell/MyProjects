using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

[ExecuteInEditMode]
public class SkillTreeSceneManager : MonoBehaviour
{
    [FoldoutGroup("Colors")]

    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color nameBackgroundColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color nameTextColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color levelBackgroundColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color levelLabelColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color levelValueColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color skillPointLabelColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color skillPointValueColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color selectionBackgroundColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color currentSelectionColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color unselectedColor;
    [FoldoutGroup("Colors/Header")]
    [ColorPalette("UI Colors")]
    public Color characterSelectionColor;

    [FoldoutGroup("Colors/Skills")]

    [FoldoutGroup("Colors/Skills/Header")]
    [ColorPalette("UI Colors")]
    public Color treeSectionBackgroundColor;
    [FoldoutGroup("Colors/Skills/Header")]
    [ColorPalette("UI Colors")]
    public Color treeSectionTextColor;
    [FoldoutGroup("Colors/Skills/Header")]
    [ColorPalette("UI Colors")]
    public Color headerBackground1Color;
    [FoldoutGroup("Colors/Skills/Header")]
    [ColorPalette("UI Colors")]
    public Color headerBackground2Color;
    [FoldoutGroup("Colors/Skills/Header")]
    [ColorPalette("UI Colors")]
    public Color headerTextColor;

    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color skillsBackground1Color;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color skillsBackground2Color;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color skillsBackground3Color;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color skillFrameColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color skillTextBacgroundColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color skillTextColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color currentSkillBackgroundColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color currentSkillValueColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color maxSkillBackgroundColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color maxSkillValueColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color currentlySelectedSkillColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color skillLineColor;
    [FoldoutGroup("Colors/Skills/Content")]
    [ColorPalette("UI Colors")]
    public Color levelRequirementColor;

    [FoldoutGroup("Images")]

    [FoldoutGroup("Images/Header")]
    public Image nameBackground;
    [FoldoutGroup("Images/Header")]
    public TextMeshProUGUI nameText;
    [FoldoutGroup("Images/Header")]
    public Image levelBackground;
    [FoldoutGroup("Images/Header")]
    public TextMeshProUGUI levelLabelText;
    [FoldoutGroup("Images/Header")]
    public TextMeshProUGUI levelValueText;
    [FoldoutGroup("Images/Header")]
    public TextMeshProUGUI skillPointLabelText;
    [FoldoutGroup("Images/Header")]
    public TextMeshProUGUI skillPointValueText;
    [FoldoutGroup("Images/Header")]
    public Image selectionBackground;
    [FoldoutGroup("Images/Header")]
    public TextMeshProUGUI characterSelectText;

    [FoldoutGroup("Images/Skills")]

    [FoldoutGroup("Images/Skills/Header")]
    public List<Image> treeSectionBackgrounds;
    [FoldoutGroup("Images/Skills/Header")]
    public List<TextMeshProUGUI> treeSectionTexts;
    [FoldoutGroup("Images/Skills/Header")]
    public List<Image> headerBackground1s;
    [FoldoutGroup("Images/Skills/Header")]
    public List<Image> headerBackgorund2s;
    [FoldoutGroup("Images/Skills/Header")]
    public List<TextMeshProUGUI> headerTexts;

    [FoldoutGroup("Images/Skills/Content")]
    public List<Image> skillsBackground1Images;
    [FoldoutGroup("Images/Skills/Content")]
    public List<Image> skillsBackground2Images;
    [FoldoutGroup("Images/Skills/Content")]
    public List<Image> skillsBackground3Images;

    public GameObject rootSkillTreeGameObject;

    private SkillTreeUI skillTreeUI;

    private void SetColor(Image image, Color color)
    {
        image.color = color;
        EditorUtility.SetDirty(image);
    }

    private void SetColor(TextMeshProUGUI text, Color color)
    {
        text.color = color;
        EditorUtility.SetDirty(text);
    }

    private void SetColor(List<Image> images, Color color)
    {
        foreach(Image image in images)
        {
            SetColor(image, color);
        }
    }

    private void SetColor(List<TextMeshProUGUI> texts, Color color)
    {
        foreach(TextMeshProUGUI text in texts)
        {
            SetColor(text, color);
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!skillTreeUI)
        {
            skillTreeUI = rootSkillTreeGameObject.GetComponent<SkillTreeUI>();
            if (!skillTreeUI)
            {
                skillTreeUI = rootSkillTreeGameObject.GetComponentInChildren<SkillTreeUI>();
            }
        }
        SquareLineDrawerUI[] lines = rootSkillTreeGameObject.GetComponentsInChildren<SquareLineDrawerUI>();
        RequirementsContainer[] requirements = rootSkillTreeGameObject.GetComponentsInChildren<RequirementsContainer>();

        SetColor(nameBackground, nameBackgroundColor);
        SetColor(nameText, nameTextColor);
        SetColor(levelBackground, levelBackgroundColor);
        SetColor(levelLabelText, levelLabelColor);
        SetColor(levelValueText, levelValueColor);
        SetColor(skillPointLabelText, skillPointLabelColor);
        SetColor(skillPointValueText, skillPointValueColor);
        SetColor(selectionBackground, selectionBackgroundColor);
        SetColor(characterSelectText, characterSelectionColor);
        SetColor(treeSectionBackgrounds, treeSectionBackgroundColor);
        SetColor(treeSectionTexts, treeSectionTextColor);
        SetColor(headerBackground1s, headerBackground1Color);
        SetColor(headerBackgorund2s, headerBackground2Color);
        SetColor(headerTexts, headerTextColor);
        SetColor(skillsBackground1Images, skillsBackground1Color);
        SetColor(skillsBackground2Images, skillsBackground2Color);
        SetColor(skillsBackground3Images, skillsBackground3Color);

        foreach (SquareLineDrawerUI line in lines)
        {
            line.color = skillLineColor;
            EditorUtility.SetDirty(line);
        }

        foreach (RequirementsContainer container in requirements)
        {
            TextMeshProUGUI[] texts = container.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                SetColor(text, levelRequirementColor);
            }
        }

        skillTreeUI.validNode.one = skillFrameColor;
        skillTreeUI.validNode.two = skillTextBacgroundColor;
        skillTreeUI.validNode.three = maxSkillBackgroundColor;

        skillTreeUI.selectedNodeColor = currentlySelectedSkillColor;

        skillTreeUI.UpdateNodes();
    }
#endif
}

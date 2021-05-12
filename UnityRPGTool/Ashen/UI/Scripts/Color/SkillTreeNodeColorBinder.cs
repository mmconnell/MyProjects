using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillTreeNodeColorBinder : A_ColorThemeBinder
{
    private SkillTreeUI skillTreeUI;

    [FoldoutGroup("Valid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string validNodeAccentA;
    [FoldoutGroup("Valid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string validNodeAccentB;
    [FoldoutGroup("Valid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string validNodeAccentC;

    [FoldoutGroup("Invalid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string invalidNodeAccentA;
    [FoldoutGroup("Invalid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string invalidNodeAccentB;
    [FoldoutGroup("Invalid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string invalidNodeAccentC;

    [FoldoutGroup("Selected Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string selectedNodeAccentA;

    [FoldoutGroup("Requirements")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string lineColor;
    [FoldoutGroup("Requirements")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string requirementsColor;

    [FoldoutGroup("Selection")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string activeSelection;
    [FoldoutGroup("Selection")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string inactiveSelection;

    private Color cachedValidAccentA;
    private Color cachedValidAccentB;
    private Color cachedValidAccentC;
    private Color cachedInvalidAccentA;
    private Color cachedInvalidAccentB;
    private Color cachedInvalidAccentC;
    private Color cachedSelectedAccentA;
    private Color cachedSelectedAccentB;
    private Color cachedSelectedAccentC;
    private Color cachedActiveSelection;
    private Color cachedInactiveSelection;

    protected override void InternalUpdateColor()
    {
        if (!skillTreeUI)
        {
            skillTreeUI = GetComponent<SkillTreeUI>();
        }
        Color validA = colorThemeManager.colorMap[validNodeAccentA];
        if (cachedValidAccentA != validA)
        {
            skillTreeUI.validNode.one = validA;
            cachedValidAccentA = validA;
        }
        Color validB = colorThemeManager.colorMap[validNodeAccentB];
        if (cachedValidAccentB != validB)
        {
            skillTreeUI.validNode.two = validB;
            cachedValidAccentB = validB;
        }
        Color validC = colorThemeManager.colorMap[validNodeAccentC];
        if (cachedValidAccentC != validC)
        {
            skillTreeUI.validNode.three = validC;
            cachedValidAccentC = validC;
        }

        Color invalidA = colorThemeManager.colorMap[invalidNodeAccentA];
        if (cachedInvalidAccentA != invalidA)
        {
            skillTreeUI.invalidNode.one = invalidA;
            cachedInvalidAccentA = invalidA;
        }
        Color invalidB = colorThemeManager.colorMap[invalidNodeAccentB];
        if (cachedInvalidAccentB != invalidB)
        {
            skillTreeUI.invalidNode.two = invalidB;
            cachedInvalidAccentB = invalidB;
        }
        Color invalidC = colorThemeManager.colorMap[invalidNodeAccentC];
        if (cachedInvalidAccentC != invalidC)
        {
            skillTreeUI.invalidNode.three = invalidC;
            cachedInvalidAccentC = invalidC;
        }

        Color selectedA = colorThemeManager.colorMap[selectedNodeAccentA];
        if (cachedSelectedAccentA != selectedA)
        {
            skillTreeUI.selectedNodeColor = selectedA;
            cachedSelectedAccentA = selectedA;
        }

        Color activeSelection = colorThemeManager.colorMap[this.activeSelection];
        if (cachedActiveSelection != activeSelection)
        {
            skillTreeUI.selectedIndicator = activeSelection;
            cachedActiveSelection = activeSelection;
        }
        Color inactiveSelection = colorThemeManager.colorMap[this.inactiveSelection];
        if (cachedInactiveSelection != inactiveSelection)
        {
            skillTreeUI.inactiveIndicator = inactiveSelection;
            cachedInactiveSelection = inactiveSelection;
        }

        skillTreeUI.UpdateNodes();

        Color lineColor = colorThemeManager.colorMap[this.lineColor];
        SquareLineDrawerUI[] lines = gameObject.GetComponentsInChildren<SquareLineDrawerUI>();
        foreach (SquareLineDrawerUI line in lines)
        {
            if (line.color != lineColor)
            {
                line.color = lineColor;
                EditorUtility.SetDirty(line);
            }
        }

        Color requirementsColor = colorThemeManager.colorMap[this.requirementsColor];
        RequirementsPositionController[] requirements = gameObject.GetComponentsInChildren<RequirementsPositionController>();
        foreach (RequirementsPositionController requirement in requirements)
        {
            Graphic graphic = requirement.GetComponent<Graphic>();
            if (graphic != null)
            {
                if (graphic.color != requirementsColor)
                {
                    graphic.color = requirementsColor;
                    EditorUtility.SetDirty(graphic);
                }
            }
        }
    }
}

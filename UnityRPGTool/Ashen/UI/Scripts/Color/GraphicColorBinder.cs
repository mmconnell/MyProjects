using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;
using UnityEditor;

public class GraphicColorBinder : A_ColorThemeBinder
{
    [ValueDropdown(nameof(GetColorThemes))]
    public string colorThemeElement;
    [PropertyRange(0, 1)]
    public float opacity = 1;

    private Color cachedColor;

    private Graphic graphic;

    protected override void InternalUpdateColor()
    {
        if (!graphic)
        {
            graphic = GetComponent<Graphic>();
        }
        if (!graphic)
        {
            return;
        }
        if (!colorThemeManager.colorMap.ContainsKey(colorThemeElement))
        {
            throw new Exception(colorThemeElement + " could not be found in the list of available colors");
        }
        Color color = colorThemeManager.colorMap[colorThemeElement];
        color.a = opacity;
        if (cachedColor != color)
        {
            graphic.color = color;
            EditorUtility.SetDirty(graphic);
            cachedColor = color;
        }
    }
}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class RequirementsContainer : MonoBehaviour
{
    public List<RequirementsPositionController> controllers;
    public RequirementsPositionController focus;
    public SkillTreeNodeUI source;
    public DisplayType displayType;

    public void Reset()
    {
        if (displayType == DisplayType.Combine)
        {
            focus = null;
            foreach (RequirementsPositionController controller in controllers)
            {
                if (!controller.disable)
                {
                    focus = controller;
                    focus.hide = false;
                }
            }
            foreach (RequirementsPositionController controller in controllers)
            {
                if (controller != focus)
                {
                    controller.hide = true;
                }
            }
        }
        else
        {
            foreach (RequirementsPositionController controller in controllers)
            {
                if (controller.disable)
                {
                    controller.hide = true;
                }
                else
                {
                    controller.hide = false;
                }
            }
        }
    }
}

public enum DisplayType
{
    Combine, Separate
}
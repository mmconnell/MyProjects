using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SkillNodeRequirementsConfiguration
{
    public RectTransformSide requiresBound;
    public RectTransformSide sourceBound;
    [Range(0, 100)]
    public int locationX;
    [Range(0, 100)]
    public int locationY;
}

using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;
using System.Collections.Generic;
using System;

[Serializable]
public class AbilityRequirementsCost
{
    [EnumToggleButtons, HideLabel]
    public CostTypeInspector costType;

    [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
    [ShowIf(nameof(costType), CostTypeInspector.PrimaryResource)]
    public I_Equation primaryResourceValue;

    [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
    [ShowIf(nameof(costType), CostTypeInspector.Health)]
    public I_Equation healthValue;

    [ShowIf(nameof(costType), CostTypeInspector.Custom), AutoPopulate]
    public List<AbilityRequirementsCostCustom> customRequirements;
}

public enum CostTypeInspector
{
    PrimaryResource, Health, Custom
}
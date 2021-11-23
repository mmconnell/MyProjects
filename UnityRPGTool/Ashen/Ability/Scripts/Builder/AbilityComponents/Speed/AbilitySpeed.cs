using UnityEngine;
using System.Collections;
using System;
using Ashen.EquationSystem;
using Ashen.VariableSystem;
using Sirenix.OdinInspector;

[Serializable]
public class AbilitySpeed
{
    [EnumToggleButtons, HideLabel]
    public SpeedOptionInspector option;

    [ShowIf(nameof(option), Value = SpeedOptionInspector.Category)]
    public AbilitySpeedCategory speedCategory;
    [ShowIf("@" + nameof(speedCategory) + " == null || " + 
        nameof(speedCategory) + "." + nameof(AbilitySpeedCategory.useSpeedCalculation) + " || " + 
        nameof(option) + " == " + nameof(SpeedOptionInspector) + "." + nameof(SpeedOptionInspector.SpeedFactor))]
    public Reference<I_Equation> speedEquation; 

    public enum SpeedOptionInspector
    {
        SpeedFactor, Category
    }
}

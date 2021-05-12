﻿using UnityEngine;
using UnityEditor;
using Ashen.EquationSystem;

namespace Ashen.VariableSystem
{
    [CreateAssetMenu(fileName = nameof(EquationVariable), menuName = "Custom/Variables/Equation")]
    public class EquationVariable : A_Variable<I_Equation>
    { }
}
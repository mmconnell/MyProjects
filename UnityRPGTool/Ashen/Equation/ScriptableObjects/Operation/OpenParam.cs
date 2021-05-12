using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(OpenParam), menuName = "Custom/Enums/" + nameof(Operations) + "/" + nameof(OpenParam))]
    public class OpenParam : A_Operation
    {
        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments)
        {
            equation.currentIndex++;
            float innerValue = equation.Calculate(source, target, equation.currentIndex, extraArguments);
            equation.keepGoing = true;
            return innerValue;
        }

        public override string Representation()
        {
            return "(";
        }
    }
}
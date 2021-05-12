using System.Collections;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Manager;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = "_,", menuName = "Custom/Enums/Operations/Comma")]
    public class ArgumentSeparator : A_Operation
    {
        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments)
        {
            equation.keepGoing = false;
            return total;
        }

        public override string Representation()
        {
            return ",";
        }
    }
}
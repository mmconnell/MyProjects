using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(CloseParam), menuName = "Custom/Enums/Operations/" + nameof(CloseParam))]
    public class CloseParam : A_Operation
    {
        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments)
        {
            equation.keepGoing = false;
            return total;
        }

        public override string Representation()
        {
            return ")";
        }
    }
}
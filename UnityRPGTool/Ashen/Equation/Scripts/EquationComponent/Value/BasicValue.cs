using UnityEngine;
using System.Collections;
using Manager;
using System;
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    [Serializable]
    public class BasicValue : A_Value
    {
        public float value;

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments)
        {
            return value;
        }

        public override string Representation()
        {
            return value + "";
        }

        public override bool RequiresCaching()
        {
            return false;
        }

        public override bool Cache(I_DeliveryTool toolManager, Equation equation)
        {
            return true;
        }
    }
}
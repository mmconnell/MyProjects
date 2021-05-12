using UnityEngine;
using System.Collections;
using Manager;
using Sirenix.OdinInspector;
using Ashen.DeliverySystem;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEditor;
using EditorUtilities;

namespace Ashen.EquationSystem
{
    public class RangeEquation : I_Equation
    {
        [HorizontalGroup("Equation"), OdinSerialize, InlineProperty, LabelWidth(50), HideReferenceObjectPicker]
        private Equation low = default;
        [HorizontalGroup("Equation"), OdinSerialize, InlineProperty, LabelWidth(50), HideReferenceObjectPicker]
        private Equation high = default;

        [ShowInInspector, CustomSpace(spaceBefore = true), HideLabel]
        public string equation
        {
            get
            {
                return ToString();
            }
        }

        public float Calculate(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            if (low == null || high == null || low == null || high == null)
            {
                return 0f;
            }
            float lowResult = low.Calculate(source, target, extraArguments);
            float highResult = high.Calculate(source, target, extraArguments);
            float finalResult = Random.Range(lowResult, highResult);
            return finalResult;
        }

        public float Calculate(I_DeliveryTool source, EquationArgumentPack extraArguments)
        {
            return Calculate(source, null, extraArguments);
        }

        public float GetHigh(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            return high.Calculate(source, target, extraArguments);
        }

        public float GetLow(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            return low.Calculate(source, target, extraArguments);
        }

        public I_Equation Rebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArgumentPack)
        {
            RangeEquation newEquation = new RangeEquation();

            newEquation.low = low.Rebuild(source, target, equationArgumentPack) as Equation;

            newEquation.high = high.Rebuild(source, target, equationArgumentPack) as Equation;


            return newEquation;
        }

        public bool RequiresRebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArgumentPack)
        {
            return low.RequiresRebuild(source, target, equationArgumentPack) || high.RequiresRebuild(source, target, equationArgumentPack);
        }

        public override string ToString()
        {
            string strValue = "[";
            if (low != null && low != null)
            {
                strValue += low.ToString();
            }
            strValue += "]-[";
            if (high != null && high != null)
            {
                strValue += high.ToString();
            }
            strValue += "]";
            return strValue;
        }
    }
}
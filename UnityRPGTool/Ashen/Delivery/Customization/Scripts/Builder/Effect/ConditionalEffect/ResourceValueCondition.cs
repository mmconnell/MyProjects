using UnityEngine;
using System.Collections;
using Manager;
using Ashen.EquationSystem;
using Ashen.VariableSystem;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem {
    public class ResourceValueCondition : I_EffectCondition
    {
        public CompareType type;
        public ResourceValue resourceValue;
        public Comparable comparable;
        public Reference<I_Equation> equation;

        [HideIf(nameof(comparable), Comparable.EQ)]
        public bool equal;

        public bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ResourceValueTool resourceValueTool = ((DeliveryTool)target).toolManager.Get<ResourceValueTool>();
            ThresholdEventValue value = resourceValueTool.GetValue(resourceValue);
            int thresholdValue = (type == CompareType.PERCENTAGE) ? ((int)(value.currentValue / (float)value.maxValue)) : value.currentValue;
            int equationValue = (int)equation.Value.Calculate(target, deliveryArguments.GetPack<EquationArgumentPack>());

            switch (comparable)
            {
                case Comparable.EQ:
                    return equationValue == thresholdValue;
                case Comparable.GT:
                    return equal ? thresholdValue >= equationValue : thresholdValue > equationValue;
                case Comparable.LT:
                    return equal ? thresholdValue <= equationValue : thresholdValue < equationValue;
            }

            return false;
        }

        public string visualize()
        {
            string compStr = "";
            switch (comparable)
            {
                case Comparable.EQ:
                    compStr = "=";
                    break;
                case Comparable.GT:
                    compStr = ">" + (equal ? "=" : "");
                    break;
                case Comparable.LT:
                    compStr = "<" + (equal ? "=" : "");
                    break;
            }
            return resourceValue.name + " " + compStr + " " + equation.Value.ToString();
        }
    }

    public enum Comparable
    {
        GT, LT, EQ
    }

    public enum CompareType
    {
        PERCENTAGE, VALUE
    }
}
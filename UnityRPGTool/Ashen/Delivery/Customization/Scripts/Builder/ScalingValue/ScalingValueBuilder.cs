using Ashen.EquationSystem;
using Ashen.VariableSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ScalingValueBuilder
    {
        [HorizontalGroup(nameof(ScalingValueBuilder), width: 0.5f), OdinSerialize, HideLabel]
        public Reference<I_Equation> equation = default;
        [OdinSerialize, FoldoutGroup("Scaling")]
        private EffectFloatArgument multiplier;
        [OdinSerialize, FoldoutGroup("Scaling")]
        private EffectFloatArgument flat;
        
        public float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            float value = equation.Value.Calculate(owner, target, deliveryArguments.GetPack<EquationArgumentPack>());
            EffectsArgumentPack effectPack = deliveryArguments.GetPack<EffectsArgumentPack>();
            if (multiplier != null)
            {
                value *= effectPack.GetFloatScale(multiplier);
            }
            if (flat != null)
            {
                value += effectPack.GetFloatFlat(flat);
            }
            return value;
        }

        public string Visualize()
        {
            string value = equation.Value.ToString();
            if (multiplier)
            {
                value = "((" + value + ") * (" + multiplier.name + "))"; 
            }
            if (flat)
            {
                value += " + " + flat.ToString();
            }
            return value;
        }
    }
}
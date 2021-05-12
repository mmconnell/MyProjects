using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Sirenix.Serialization;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;
using Ashen.VariableSystem;

[HideLabel, BoxGroup("Ticker")]
public class GemTickerPack : I_TickerPack
{
    [FoldoutGroup("Frequency", expanded: true), OdinSerialize, HideLabel]
    private Reference<I_Equation> frequency = default;

    public I_Ticker Build(I_DeliveryTool owner, I_DeliveryTool target, EquationArgumentPack extraArguments)
    {
        return new TimeTicker(null, (int)frequency.Value.Calculate(owner, target, extraArguments));
    }
}

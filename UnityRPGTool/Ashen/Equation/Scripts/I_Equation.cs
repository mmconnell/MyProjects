using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ashen.EquationSystem
{
    [HideReferenceObjectPicker]
    public interface I_Equation
    {
        float Calculate(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArguments);
        float Calculate(I_DeliveryTool source, EquationArgumentPack equationArguments);
        float Calculate(I_DeliveryTool source);
        bool RequiresRebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArgumentPack);
        I_Equation Rebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArgumentPack);
        float GetLow(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArguments);
        float GetHigh(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack equationArguments);
    }
}
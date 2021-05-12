using UnityEngine;
using System.Collections;
using Manager;
using System;
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public interface I_EquationComponent
    {
        float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments);
        string Representation();
        bool IsOperation();
        bool IsArgumentOperation();
        bool RequiresRebuild();
        I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments);
        bool RequiresCaching();
        bool IsCachable();
        bool Cache(I_DeliveryTool source, Equation equation);
        bool InvalidComponent();
    }
}
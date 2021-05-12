using UnityEngine;
using System.Collections;
using Manager;
using System;
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    [Serializable]
    public abstract class A_Value : I_EquationComponent
    {
        public bool IsOperation()
        {
            return false;
        }

        public bool IsArgumentOperation()
        {
            return false;
        }

        public abstract string Representation();
        public abstract float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments);

        public abstract bool RequiresCaching();
        public abstract bool Cache(I_DeliveryTool toolManager, Equation equation);

        public virtual bool IsCachable()
        {
            return true;
        }

        public virtual bool RequiresRebuild()
        {
            return false;
        }

        public virtual I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            return this;
        }

        public virtual bool InvalidComponent()
        {
            return false;
        }
    }
}
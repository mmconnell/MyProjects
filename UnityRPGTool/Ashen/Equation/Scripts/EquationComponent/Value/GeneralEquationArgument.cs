﻿using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Sirenix.Serialization;

namespace Ashen.EquationSystem
{
    public class GeneralEquationArgument : A_Value
    {
        [OdinSerialize]
        private I_EquationArgument generalArgument = default;

        public GeneralEquationArgument() { }

        public GeneralEquationArgument(I_EquationArgument argument)
        {
            generalArgument = argument;
        }

        public override bool Cache(I_DeliveryTool toolManager, Equation equation)
        {
            return false;
        }

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments)
        {
            if (extraArguments == null)
            {
                return generalArgument.DefaultValue();
            }
            I_EquationArgument argument = extraArguments.GetArgument(GemLevelArgument.GEM_LEVEL_ARGUMENT);
            if (argument == null)
            {
                return generalArgument.DefaultValue();
            }
            return argument.GetValue();
        }

        public override string Representation()
        {
            return generalArgument.GetKey();
        }

        public override bool RequiresCaching()
        {
            return false;
        }

        public override bool IsCachable()
        {
            return false;
        }

        public override bool RequiresRebuild()
        {
            return true;
        }

        public override I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            BasicValue value = new BasicValue();
            if (extraArguments == null)
            {
                value.value = generalArgument.DefaultValue();
                return value;
            }
            I_EquationArgument argument = extraArguments.GetArgument(GemLevelArgument.GEM_LEVEL_ARGUMENT);
            if (argument == null)
            {
                value.value = generalArgument.DefaultValue();
                return value;
            }
            value.value = argument.GetValue();
            return value;
        }
    }
}
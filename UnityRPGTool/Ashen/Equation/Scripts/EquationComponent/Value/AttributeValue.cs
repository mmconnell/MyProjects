using UnityEngine;
using UnityEditor;
using Manager;
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public class AttributeValue : A_Value
    {
        public DerivedAttribute attribute;
        public bool useTarget;

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, EquationArgumentPack extraArguments)
        {
            if (useTarget)
            {
                return attribute.Get(target);
            }
            return attribute.Get(target);
        }

        public override string Representation()
        {
            if (attribute)
            {
                return attribute.ToString();
            }
            return "null";
        }

        public override bool IsCachable()
        {
            return !useTarget;
        }

        public override bool RequiresCaching()
        {
            return true;
        }

        public override bool RequiresRebuild()
        {
            return useTarget;
        }

        public override I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            if (!useTarget)
            {
                return this;
            }
            else
            {
                return new BasicValue
                {
                    value = attribute.Get(target)
                };
            }
        }

        public override bool Cache(I_DeliveryTool source, Equation equation)
        {
            if (IsCachable())
            {
                if (source == null)
                {
                    return false;
                }
                ToolManager toolManager = (source as DeliveryTool).toolManager;
                AttributeTool attributeTool = toolManager.Get<AttributeTool>();
                if (!attributeTool)
                {
                    return false;
                }
                attributeTool.Cache(attribute, equation);
            }
            return false;
        }
    }
}
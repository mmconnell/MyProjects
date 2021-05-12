﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(BaseAttributeParser), menuName = "Custom/EquationParser/" + nameof(BaseAttributeParser))]
    public class BaseAttributeParser : A_ComponentParser<BaseAttribute>
    {
        public override I_EquationComponent GetEquationComponent(string toParse)
        {
            List<string> arguments = new List<string>();
            if (toParse.Contains("_"))
            {
                string key = toParse.Substring(0, toParse.IndexOf('_'));
                if (toParse.IndexOf("_") < toParse.Length - 1)
                {
                    string argumentString = toParse.Substring(toParse.IndexOf('_') + 1);
                    foreach (char character in argumentString)
                    {
                        arguments.Add((character + "").ToLower());
                    }
                }
                toParse = key;
            }
            if (StringValid(toParse))
            {
                BaseAttributeValue attributeValue = new BaseAttributeValue();
                attributeValue.attribute = parseMap[toParse];
                if (arguments.Contains("t"))
                {
                    attributeValue.useTarget = true;
                }
                return attributeValue;
            }
            return null;
        }
    }
}
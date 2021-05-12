using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public class EquationArgumentPack : A_DeliveryArgumentPack<EquationArgumentPack>
    {
        private Dictionary<string, I_EquationArgument> equationArguments;

        public override I_DeliveryArgumentPack Initialize()
        {
            return new EquationArgumentPack();
        }

        public I_EquationArgument GetArgument(string key)
        {
            if (equationArguments == null)
            {
                return null;
            }
            if (equationArguments.TryGetValue(key, out I_EquationArgument value))
            {
                return value;
            }
            return null;
        }

        public void AddArgument(string key, I_EquationArgument argument)
        {
            if (equationArguments == null)
            {
                equationArguments = new Dictionary<string, I_EquationArgument>();
            }
            if (equationArguments.ContainsKey(key))
            {
                equationArguments[key] = argument;
            }
            else
            {
                equationArguments.Add(key, argument);
            }
        }

        public override void Clear()
        {
            if (equationArguments != null)
            {
                equationArguments.Clear();
            }
        }
    }
}
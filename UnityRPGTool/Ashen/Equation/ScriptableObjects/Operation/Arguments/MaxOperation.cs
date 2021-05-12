﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = "Max", menuName = "Custom/Enums/Operations/Max")]
    public class MaxOperation : A_ArgumentOperation
    {
        public override float RunOperation(List<float> args)
        {
            if (args.Count == 1)
            {
                return args[0];
            }
            if (args.Count != 0)
            {
                return Mathf.Max(args.ToArray());
            }
            return 0;
        }

        public override string Representation()
        {
            return "Max(";
        }
    }
}
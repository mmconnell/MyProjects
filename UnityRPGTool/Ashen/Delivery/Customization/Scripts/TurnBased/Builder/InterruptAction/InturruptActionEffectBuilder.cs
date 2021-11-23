﻿using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Ashen.VariableSystem;
using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    public class InterruptActionEffectBuilder : I_EffectBuilder
    {
        [OdinSerialize]
        private AbilityTag[] abilityTags;
        [OdinSerialize]
        private AbilitySO ability;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new InterruptActionEffect(ability, abilityTags);
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Retarget effect to owner if ability has any tag of [" + abilityTags.ToString() + "]";
            return vis;
        }
    }
}
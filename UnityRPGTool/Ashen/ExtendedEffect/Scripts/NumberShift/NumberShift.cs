using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A number shift allows for values to be scaled. This is useful for things like
     * Resistances being temporarily boosted or reduced by StatusEffects.
     **/
    public class NumberShift
    {
        public NumberShift()
        {
            flat = 0;
            scale = 0;
        }

        [OdinSerialize]
        public float flat;

        [OdinSerialize]
        public float scale;

        public int FlatInt()
        {
            return (int)flat;
        }

        public int ScaleInt()
        {
            return (int)scale;
        }
    }
}
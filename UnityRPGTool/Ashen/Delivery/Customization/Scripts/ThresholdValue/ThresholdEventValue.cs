using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

namespace Ashen.DeliverySystem
{
    public struct ThresholdEventValue
    {
        public int previousValue;
        public int currentValue;
        public int maxValue;
        public bool max;
        public bool min;
        public bool damageTaken;
    }
}
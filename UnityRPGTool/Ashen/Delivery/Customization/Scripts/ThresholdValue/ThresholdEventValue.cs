using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Manager;

namespace Ashen.DeliverySystem
{
    public struct ThresholdEventValue
    {
        public ResourceValue resourceValue;
        public int previousValue;
        public int currentValue;
        public int maxValue;
        public bool max;
        public bool min;
        public bool damageTaken;
    }
}
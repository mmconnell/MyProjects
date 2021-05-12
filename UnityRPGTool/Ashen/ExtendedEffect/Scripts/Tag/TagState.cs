using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public struct TagState
    {
        public bool continueOperation;
        public bool validStatusEffect;
        public List<ExtendedEffectTag> appliedTags;
    }
}
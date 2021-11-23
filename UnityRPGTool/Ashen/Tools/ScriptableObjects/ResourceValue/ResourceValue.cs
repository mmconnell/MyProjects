using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;

namespace Manager
{
    [CreateAssetMenu(fileName = nameof(ResourceValue), menuName = "Custom/Enums/" + nameof(ResourceValues) + "/Type")]
    public class ResourceValue : A_EnumSO<ResourceValue, ResourceValues>
    {
        [EnumSODropdown]
        public List<DamageType> listenOn;
        [Hide, Title("Threshold")]
        public ThresholdBuilder threshold;
        public Color color1;
        public Color color2;
        public string displayName;
    }
}
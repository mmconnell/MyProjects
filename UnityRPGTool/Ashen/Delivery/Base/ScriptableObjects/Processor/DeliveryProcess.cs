using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(DeliveryProcess), menuName = "Custom/CombatInfrastructure/" + nameof(DeliveryProcess))]
    public class DeliveryProcess : SerializedScriptableObject
    {
        public I_DeliveryProcessor processor;
    }
}
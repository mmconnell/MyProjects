using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(DeliveryProcessorContainer), menuName = "Custom/CombatInfrastructure/" + nameof(DeliveryProcessorContainer))]
    public class DeliveryProcessorContainer : SerializedScriptableObject
    {
        public DeliveryProcess[] processors;
    }
}
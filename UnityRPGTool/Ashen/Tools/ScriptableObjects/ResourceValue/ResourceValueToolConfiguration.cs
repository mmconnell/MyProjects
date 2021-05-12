using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.UI;
using Sirenix.Serialization;

namespace Manager
{
    [CreateAssetMenu(fileName = "ResourceValueToolConfiguration", menuName = "Custom/Tool/ResourceValueToolConfiguration")]
    public class ResourceValueToolConfiguration : SerializedScriptableObject
    {
        [OdinSerialize]
        public Dictionary<ResourceValue, Slider> unitySliderListeners;
        public Dictionary<ResourceValue, Slider> UnitySliderListeners
        {
            get
            {
                if (unitySliderListeners != null)
                {
                    return unitySliderListeners;
                }
                return new Dictionary<ResourceValue, Slider>();
            }

        }
    }
}
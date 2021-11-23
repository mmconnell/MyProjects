using UnityEngine;
using Sirenix.OdinInspector;

namespace Manager
{
    [CreateAssetMenu(fileName = "ResourceValueToolConfiguration", menuName = "Custom/Tool/ResourceValueToolConfiguration")]
    public class ResourceValueToolConfiguration : SerializedScriptableObject
    {
        [SerializeField]
        private ResourceValue defaultAbilityResource;
        public ResourceValue DefaultAbilityResource
        {
            get
            {
                if (!defaultAbilityResource)
                {
                    if (this == DefaultValues.Instance.defaultResourceValueToolConfiguration)
                    {
                        return null;
                    }
                    return DefaultValues.Instance.defaultResourceValueToolConfiguration.DefaultAbilityResource;
                }
                return defaultAbilityResource;
            }
        }
    }
}
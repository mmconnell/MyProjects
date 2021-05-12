using UnityEngine;
using System.Collections;

namespace Manager
{
    [CreateAssetMenu(fileName = nameof(ResourceValues), menuName = "Custom/Enums/" + nameof(ResourceValues) + "/Types")]
    public class ResourceValues : A_EnumList<ResourceValue, ResourceValues>
    {
        public ResourceValue health;
    }
}
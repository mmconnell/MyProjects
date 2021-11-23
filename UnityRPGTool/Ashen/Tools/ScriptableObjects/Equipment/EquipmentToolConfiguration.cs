using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Sirenix.Serialization;

namespace Manager {
    public class EquipmentToolConfiguration : SerializedScriptableObject
    {
        [OdinSerialize, EnumSODropdown]
        private List<DamageType> baseDamageTypes;

        public List<DamageType> BaseDamageTypes
        {
            get
            {
                if (baseDamageTypes == null)
                {
                    if (DefaultValues.Instance.defaultEquipmentToolConfiguration == this)
                    {
                        return new List<DamageType>();
                    }
                    return DefaultValues.Instance.defaultEquipmentToolConfiguration.BaseDamageTypes;
                }
                return baseDamageTypes;
            }
        }
    }
}
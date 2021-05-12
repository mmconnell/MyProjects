using UnityEngine;
using UnityEditor;
using Manager;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class ClearThresholdValue : A_SimpleComponent
    {
        [HideLabel, EnumSODropdownAttribute, Title("Damage Type")]
        public ResourceValue resourceValue;

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deilveryTool = dse.target as DeliveryTool;
            ResourceValueTool resourceValueTool = deilveryTool.toolManager.Get<ResourceValueTool>();
            if (resourceValueTool)
            {
                resourceValueTool.ClearThresholdValue(resourceValue);
            }
        }
    }
}
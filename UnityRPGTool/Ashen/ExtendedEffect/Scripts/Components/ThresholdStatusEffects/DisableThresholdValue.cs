using UnityEngine;
using UnityEditor;
using Manager;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class DisableThresholdValue : A_SimpleComponent
    {
        [EnumSODropdownAttribute, HideLabel, Title("Damage Type")]
        public ResourceValue resourceValue;

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deilveryTool = dse.target as DeliveryTool;
            ResourceValueTool resourceValueTool = deilveryTool.toolManager.Get<ResourceValueTool>();
            if (resourceValueTool)
            {
                resourceValueTool.DisableThresholdValue(resourceValue);
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deilveryTool = dse.target as DeliveryTool;
            ResourceValueTool resourceValueTool = deilveryTool.toolManager.Get<ResourceValueTool>();
            if (resourceValueTool)
            {
                resourceValueTool.EnableThresholdValue(resourceValue);
            }
        }
    }
}
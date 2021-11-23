using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Manager;

namespace Ashen.DeliverySystem
{
    public class UpdatePrimaryResourceFilterBuilder : I_FilterBuilder
    {
        [HideLabel, EnumToggleButtons]
        public PrimaryResourceFilterType type;
        [HideLabel, EnumToggleButtons, Title("Target?")]
        public TargetChoice targetChoice;
        [OdinSerialize, Hide]
        private ScalingValueBuilder value;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            float val = value.Build(owner, target, arguments);
            ToolManager toolManager = targetChoice == TargetChoice.Owner ? (owner as DeliveryTool).toolManager : (target as DeliveryTool).toolManager;
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            return new UpdateResourceFilter(rvTool.AbilityResourceValue, val, val > 0, type == PrimaryResourceFilterType.Percentage, targetChoice == TargetChoice.Target);
        }
    }

    public enum PrimaryResourceFilterType
    {
        Percentage, Flat
    }
}
using UnityEngine;
using System.Collections;
using Manager;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    public class TargetRangeAttributeComponent : A_SimpleComponent, ISerializable
    {
        private TargetAttribute attribute;
        private TargetRange range;
        private int priority;

        public TargetRangeAttributeComponent() { }

        public TargetRangeAttributeComponent(TargetAttribute attribute, TargetRange range, int priority)
        {
            this.attribute = attribute;
            this.range = range;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool dTarget = dse.target as DeliveryTool;
            if (dTarget)
            {
                TargetAttributeTool targetAttributeTool = dTarget.toolManager.Get<TargetAttributeTool>();
                if (targetAttributeTool)
                {
                    targetAttributeTool.AddRangeShift(attribute, priority, container.key, range);
                }
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool dTarget = dse.target as DeliveryTool;
            if (dTarget)
            {
                TargetAttributeTool targetAttributeTool = dTarget.toolManager.Get<TargetAttributeTool>();
                if (targetAttributeTool)
                {
                    targetAttributeTool.RemoveRangeShift(attribute, container.key);
                }
            }
        }

        public TargetRangeAttributeComponent(SerializationInfo info, StreamingContext context)
        {
            attribute = TargetAttributes.Instance[info.GetInt32(nameof(attribute))];
            priority = info.GetInt32(nameof(priority));
            range = (TargetRange)info.GetInt32(nameof(range));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(attribute), (int)attribute);
            info.AddValue(nameof(priority), priority);
            info.AddValue(nameof(range), (int)range);
        }
    }
}

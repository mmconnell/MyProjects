using UnityEngine;
using System.Collections;
using Manager;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    public class TargetAttributeComponent : A_SimpleComponent, ISerializable
    {
        private TargetAttribute attribute;
        private Target target;
        private int priority;

        public TargetAttributeComponent() { }

        public TargetAttributeComponent(TargetAttribute attribute, Target target, int priority)
        {
            this.attribute = attribute;
            this.target = target;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool dTarget = dse.target as DeliveryTool;
            if (dTarget)
            {
                TargetAttributeTool targetAttributeTool = dTarget.toolManager.Get<TargetAttributeTool>();
                if (targetAttributeTool)
                {
                    targetAttributeTool.AddTargetShift(attribute, priority, container.key, target);
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
                    targetAttributeTool.RemoveTargetShift(attribute, container.key);
                }
            }
        }

        public TargetAttributeComponent(SerializationInfo info, StreamingContext context)
        {
            attribute = TargetAttributes.Instance[info.GetInt32(nameof(attribute))];
            priority = info.GetInt32(nameof(priority));
            target = Targets.Instance[info.GetInt32(nameof(target))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(attribute), (int)attribute);
            info.AddValue(nameof(priority), priority);
            info.AddValue(nameof(target), (int)target);
        }
    }
}

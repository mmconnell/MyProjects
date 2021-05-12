using Ashen.EquationSystem;
using Manager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Ashen.VariableSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    [Serializable]
    public class AttributeComponent : A_SimpleComponent, ISerializable
    {
        private DerivedAttribute attributeType = default;
        private ShiftCategory shiftCategory = default;
        private float value;

        public AttributeComponent() { }

        public AttributeComponent(DerivedAttribute attribute, ShiftCategory shiftCategory, float value)
        {
            this.attributeType = attribute;
            this.shiftCategory = shiftCategory;
            this.value = value;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool dTarget = dse.target as DeliveryTool;
            if (dTarget)
            {
                AttributeTool attributeTool =dTarget.toolManager.Get<AttributeTool>();
                if (attributeTool)
                {
                    attributeTool.AddShift(attributeType, shiftCategory, container.key, (int)value);
                }
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool dTarget = dse.target as DeliveryTool;
            if (dTarget)
            {
                AttributeTool attributeTool = dTarget.toolManager.Get<AttributeTool>();
                if (attributeTool)
                {
                    attributeTool.RemoveShift(attributeType, shiftCategory, container.key);
                }
            }
        }

        public AttributeComponent(SerializationInfo info, StreamingContext context)
        {
            attributeType = DerivedAttributes.Instance[info.GetInt32(nameof(attributeType))];
            shiftCategory = ShiftCategories.Instance[info.GetInt32(nameof(shiftCategory))];
            value = (float)info.GetValue(nameof(value), typeof(float));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(attributeType), (int)attributeType);
            info.AddValue(nameof(shiftCategory), (int)shiftCategory);
            info.AddValue(nameof(value), value);
        }
    }
}
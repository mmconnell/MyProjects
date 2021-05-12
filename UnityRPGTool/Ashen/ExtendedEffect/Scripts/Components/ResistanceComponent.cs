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
     * This BaseStatusEffect will reduce or raise a characters 
     * Resistance for a certain period of time
     **/
     [Serializable]
    public class ResistanceComponent : A_SimpleComponent, ISerializable
    {
        public DamageType ResistanceType = default;
        public ShiftCategory shiftCategory = default;
        public float value;

        public ResistanceComponent() { }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            if (deliveryTool)
            {
                ResistanceTool resistanceTool = deliveryTool.toolManager.Get<ResistanceTool>();
                if (resistanceTool)
                {
                    resistanceTool.AddShift(ResistanceType, shiftCategory, container.key, (int)value);
                }
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            if (deliveryTool)
            {
                ResistanceTool resistanceTool = deliveryTool.toolManager.Get<ResistanceTool>();
                if (resistanceTool)
                {
                    resistanceTool.RemoveShift(ResistanceType, shiftCategory, container.key);
                }
            }
        }

        public ResistanceComponent(SerializationInfo info, StreamingContext context)
        {
            value = (float)info.GetValue(nameof(value), typeof(float));
            ResistanceType = DamageTypes.Instance[info.GetInt32(nameof(ResistanceType))];
            shiftCategory = ShiftCategories.Instance[info.GetInt32(nameof(shiftCategory))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(value), value);
            info.AddValue(nameof(ResistanceType), (int)ResistanceType);
            info.AddValue(nameof(shiftCategory), (int)shiftCategory);
        }
    }
}
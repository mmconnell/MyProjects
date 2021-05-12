using UnityEngine;
using System.Collections;
using Manager;
using Ashen.DeliverySystem;
using System.Collections.Generic;
using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    public class ShiftableEquation
    {
        public Equation BaseValue { get; set; }

        public ShiftPack[] shifts;
        public bool allowScale;

        public virtual void Initialize()
        {
            shifts = new ShiftPack[ShiftCategories.Count];
            for (int x = 0; x < ShiftCategories.Count; x++)
            {
                ShiftPack shiftPack = new ShiftPack();
                shifts[x] = shiftPack;
                shiftPack.Initialize();
            }
        }

        public void ApplyShift(ShiftCategory shiftCategory, string source, float flat, float scale)
        {
            shifts[(int)shiftCategory].Apply(source, flat, scale);
        }

        public void RemoveShift(ShiftCategory shiftCategory, string source)
        {
            shifts[(int)shiftCategory].Clear(source);
        }

        public float GetValue(I_DeliveryTool toolManager, EquationArgumentPack extraArguments)
        {
            return Calculate(toolManager, extraArguments);
        }

        public float Calculate(I_DeliveryTool toolManager, EquationArgumentPack extraArguments)
        {
            float total = GetBase(toolManager, extraArguments);
            for (int x = 0; x < ShiftCategories.Count; x++)
            {
                ShiftPack shiftPack = shifts[x];
                if (allowScale)
                {
                    total *= (1f + shiftPack.GetMultiplier());
                }
                total += shiftPack.GetFlat();
            }
            return total;
        }

        public ShiftableEquation Copy()
        {
            ShiftableEquation shiftable = new ShiftableEquation();
            shiftable.allowScale = allowScale;
            shiftable.shifts = new ShiftPack[shifts.Length];
            for (int x = 0; x < shifts.Length; x++)
            {
                shiftable.shifts[x] = shifts[x].Copy();
            }
            return shiftable;
        }

        public float GetBase(I_DeliveryTool toolManager, EquationArgumentPack extraArguments)
        {
            return BaseValue.Calculate(toolManager, extraArguments);
        }
    }
}
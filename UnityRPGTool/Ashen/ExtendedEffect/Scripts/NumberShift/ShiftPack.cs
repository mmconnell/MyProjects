using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class ShiftPack
    {
        private static readonly int initialIndexSize = 20;
        private Dictionary<string, int> consumedShifts;

        private List<int> availableIndecies;
        private List<int> occupiedIndecies;
        private NumberShift[] numberShifts;

        private NumberShift finalShift = new NumberShift();

        public bool hasMin;
        [ShowIf("hasMin")]
        public float min;
        public bool hasMax;
        [ShowIf("hasMax")]
        public float max;

        public void Initialize()
        {
            availableIndecies = new List<int>(initialIndexSize);
            occupiedIndecies = new List<int>(initialIndexSize);
            consumedShifts = new Dictionary<string, int>();
            numberShifts = new NumberShift[initialIndexSize];
            for (int x = 0; x < initialIndexSize; x++)
            {
                availableIndecies.Add(x);
                numberShifts[x] = new NumberShift();
            }
        }

        public void Apply(string source, float flat, float scale)
        {
            if (!consumedShifts.ContainsKey(source))
            {
                if (availableIndecies.Count == 0)
                {
                    NumberShift[] tempShifts = new NumberShift[numberShifts.Length * 2];
                    availableIndecies.Capacity = tempShifts.Length;
                    for (int x = 0; x < numberShifts.Length; x++)
                    {
                        tempShifts[x] = numberShifts[x];
                    }
                    for (int x = numberShifts.Length; x < tempShifts.Length; x++)
                    {
                        availableIndecies.Add(x);
                        tempShifts[x] = new NumberShift();
                    }
                    numberShifts = tempShifts;
                }
                int index = availableIndecies[availableIndecies.Count - 1];
                consumedShifts.Add(source, index);
                availableIndecies.RemoveAt(availableIndecies.Count - 1);
                occupiedIndecies.Add(index);
                NumberShift shiftToConsume = numberShifts[index];
                shiftToConsume.flat = flat;
                shiftToConsume.scale = scale;
                EnsureValues();
            }
        }

        public void Clear(string source)
        {
            if (consumedShifts.TryGetValue(source, out int index))
            {
                UnorderedListUtility<int>.RemoveAt(occupiedIndecies, occupiedIndecies.IndexOf(index));
                availableIndecies.Add(index);
                consumedShifts.Remove(source);
                EnsureValues();
            }
        }

        public float GetFlat()
        {
            return finalShift.flat;
        }

        public float GetMultiplier()
        {
            return finalShift.scale;
        }

        private void EnsureValues()
        {
            float finalFlat = 0f;
            float finalScale = 0f;
            for (int x = 0; x < occupiedIndecies.Count; x++)
            {
                int index = occupiedIndecies[x];
                NumberShift shift = numberShifts[index];
                finalFlat += shift.flat;
                finalScale += shift.scale;
            }
            finalShift.flat = finalFlat;
            finalShift.scale = finalScale;
        }

        public ShiftPack Copy()
        {
            ShiftPack shiftPack = new ShiftPack();
            shiftPack.Initialize();
            shiftPack.hasMin = hasMin;
            shiftPack.min = min;
            shiftPack.hasMax = hasMax;
            shiftPack.max = max;
            return shiftPack;
        }
    }
}
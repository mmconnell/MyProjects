using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using Ashen.DeliverySystem;

[CreateAssetMenu(fileName = "AttributeToolConfiguration", menuName = "Custom/Tool/AttributeToolConfiguration")]
public class AttributeToolConfiguration : SerializedScriptableObject
{ 
    [OdinSerialize]
    private ShiftableEquation shiftableEquation = default;
    
    public ShiftableEquation ShiftableEquation
    {
        get
        {
            if (shiftableEquation == null)
            {
                return DefaultValues.Instance.defaultAttributeToolConfiguration.ShiftableEquation;
            }
            else
            {
                ShiftableEquation derivedShiftableEquation = shiftableEquation.Copy();
                ShiftPack[] shifts = derivedShiftableEquation.shifts;
                foreach (ShiftCategory shiftCategory in ShiftCategories.Instance)
                {
                    if (!(shifts.Length < (int)shiftCategory) || shifts[(int)shiftCategory] == null)
                    {
                        if (this != DefaultValues.Instance.defaultAttributeToolConfiguration)
                        {
                            shifts[(int)shiftCategory] = DefaultValues.Instance.defaultAttributeToolConfiguration.shiftableEquation.shifts[(int)shiftCategory];
                        }
                    }
                }
                return derivedShiftableEquation;
            }
        }
    }
}

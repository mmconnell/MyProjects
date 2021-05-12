using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using Ashen.DeliverySystem;

[CreateAssetMenu(fileName = "ResistanceToolConfiguration", menuName = "Custom/Tool/ResistanceToolConfiguration")]
public class ResistanceToolConfiguration : SerializedScriptableObject
{
    [OdinSerialize]
    private ShiftableEquation shiftableEquation = default;
    [OdinSerialize]
    private Dictionary<DamageType, DerivedAttribute> resistanceEquations = default;

    
    public ShiftableEquation ShiftableEquation
    {
        get
        {
            if (shiftableEquation == null)
            {
                return DefaultValues.Instance.defaultResistanceToolConfiguration.ShiftableEquation;
            }
            else
            {
                ShiftableEquation derivedShiftableEquation = shiftableEquation.Copy();
                ShiftPack[] shifts = derivedShiftableEquation.shifts;
                foreach (ShiftCategory shiftCategory in ShiftCategories.Instance)
                {
                    if (!(shifts.Length < (int)shiftCategory) || shifts[(int)shiftCategory] == null)
                    {
                        if (this != DefaultValues.Instance.defaultResistanceToolConfiguration)
                        {
                            shifts[(int)shiftCategory] = DefaultValues.Instance.defaultResistanceToolConfiguration.shiftableEquation.shifts[(int)shiftCategory];
                        }
                    }
                }
                return derivedShiftableEquation;
            }
        }
    }

    private Dictionary<DamageType, DerivedAttribute> derivedResistanceEquations;
    public Dictionary<DamageType, DerivedAttribute> ResistanceEquations
    {
        get
        {
            if (resistanceEquations == null)
            {
                if (this == DefaultValues.Instance.defaultResistanceToolConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultResistanceToolConfiguration.ResistanceEquations;
            }
            else
            {
                Dictionary<DamageType, DerivedAttribute> derivedResistanceEquations = new Dictionary<DamageType, DerivedAttribute>();
                foreach (DamageType damageType in DamageTypes.Instance)
                {
                    if (resistanceEquations.ContainsKey(damageType))
                    {
                        derivedResistanceEquations.Add(damageType, resistanceEquations[damageType]);
                    }
                    else
                    {
                        if (this != DefaultValues.Instance.defaultResistanceToolConfiguration)
                        {
                            derivedResistanceEquations.Add(damageType, DefaultValues.Instance.defaultResistanceToolConfiguration.resistanceEquations[damageType]);
                        }
                    }
                }
                return derivedResistanceEquations;
            }
        }
    }
}
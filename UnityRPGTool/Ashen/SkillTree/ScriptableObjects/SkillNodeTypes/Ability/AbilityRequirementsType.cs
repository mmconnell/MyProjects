using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class AbilityRequirementsType : MonoBehaviour
{
    [HideLabel, EnumToggleButtons]
    public AbilityRequirementsTypeInspector type;
    [ShowIf(nameof(type), AbilityRequirementsTypeInspector.Override), Title("Requirement"), HideLabel]
    public I_AbilityRequirement requirement;
}

public enum AbilityRequirementsTypeInspector
{
    Remove, Override
}
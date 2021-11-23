using Sirenix.OdinInspector;
using Manager;
using Ashen.EquationSystem;
using System;

[Serializable]
public class AbilityRequirementsCostCustom
{
    [EnumSODropdown, HideLabel, Title("Resource Type")]
    public ResourceValue resourceValue;
    [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
    public I_Equation customResourceValue;
}

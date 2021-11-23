using UnityEngine;
using Manager;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using static AbilityBuilder;
using Ashen.EquationSystem;

public interface I_AbilityAction
{
    void SetEffectFloat(A_EffectFloatArgument argument, float value);
    void FillDeliveryArguments(DeliveryArgumentPacks deliveryArguments);
    Target GetTargetType(ToolManager toolManager);
    TargetRange GetTargetRange(ToolManager toolManager);
    List<AbilityTag> GetAbilityTags(ToolManager toolManager);
    DeliveryPackBuilder GetDeliveryPack();
    GameObject GetAnimation();
    TargetParty GetTargetParty();
    I_Equation GetSpeedFactor();
    AbilitySpeedCategory GetSpeedCategory();
    string GetName();
    bool IsValid(ToolManager toolManager);
    int GetResourceChange(ResourceValue resourceValue, ToolManager toolManager);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_CombatAction
{
    float GetSpeedFactor();
    //CombatOption GetCombatOption();
    bool ResolveSelection();
    void ExecuteAction();
    //List<TargetType> GetPossibleTargetTypes();
    //TargetOption GetTargetOption();
}

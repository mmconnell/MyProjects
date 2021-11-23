using UnityEngine;
using System.Collections;

public interface I_CombatProcessor
{
    IEnumerator Execute(CombatProcessorInfo info);
    bool IsValid(CombatProcessorInfo info);
    bool IsFinished(CombatProcessorInfo info);
}

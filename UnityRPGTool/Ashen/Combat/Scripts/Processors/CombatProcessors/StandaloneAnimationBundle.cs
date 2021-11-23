using UnityEngine;
using System.Collections;

public class StandaloneAnimationBundle : I_CombatProcessor
{
    public AnimationExecutable animationExecutable;

    private bool isValid = true;

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        yield return animationExecutable.Execute(info.runner);
        isValid = false;
    }

    public bool IsFinished(CombatProcessorInfo info)
    {
        if (!animationExecutable.IsFinished())
        {
            return false;
        }
        return true;
    }

    public bool IsValid(CombatProcessorInfo info)
    {
        return isValid;
    }
}

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DoTweenObjectProcessor : I_CombatProcessor
{
    public Tween tween;
    public float waitTime;

    private bool isValid = true;

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        tween.Rewind();
        tween.Play();
        yield return new WaitForSeconds(waitTime);
        isValid = false;
    }

    public bool IsFinished(CombatProcessorInfo info)
    {
        return !tween.IsPlaying();
    }

    public bool IsValid(CombatProcessorInfo info)
    {
        return isValid;
    }
}

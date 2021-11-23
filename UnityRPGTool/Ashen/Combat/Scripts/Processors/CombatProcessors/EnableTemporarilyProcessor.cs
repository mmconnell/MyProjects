using UnityEngine;
using System.Collections;

public class EnableTemporarilyProcessor : I_CombatProcessor
{
    public MonoBehaviour toEnable;
    public float totalTime;

    private float currentTIme;

    private bool isValid = true;
    private bool isFinished = false;

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        toEnable.enabled = true;
        currentTIme = 0;
        isValid = false;
        info.runner.StartCoroutine(Timer());
        yield break;
    }

    public IEnumerator Timer()
    {
        while (currentTIme < totalTime)
        {
            currentTIme += Time.deltaTime;
            yield return null;
        }
        toEnable.enabled = false;
        isFinished = true;
    }

    public bool IsFinished(CombatProcessorInfo info)
    {
        return isFinished;
    }

    public bool IsValid(CombatProcessorInfo info)
    {
        return isValid;
    }
}

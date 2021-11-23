using UnityEngine;
using System.Collections;

public class CombatLogProcessor : I_CombatProcessor
{
    public string message;

    private bool isValid = true;
    private bool isFinished = false;

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        CombatLog.Instance.AddMessage(message);
        info.runner.StartCoroutine(DisplayWait());
        isValid = false;
        yield break;
    }

    private IEnumerator DisplayWait()
    {
        yield return new WaitForSeconds(1f);
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

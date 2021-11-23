using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListActionBundle : I_CombatProcessor
{
    private List<I_CombatProcessor> bundles;
    public List<I_CombatProcessor> Bundles
    {
        get
        {
            if (bundles == null)
            {
                bundles = new List<I_CombatProcessor>();
            }
            return bundles;
        }
    }

    private bool isValid = true;
    private bool isFinished = false;

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        info.parentProcessorList.InsertRange(0, Bundles);
        isValid = false;
        isFinished = true;
        yield break;
        //if (cur >= Bundles.Count)
        //{
        //    isValid = false;
        //    yield break;
        //}
        //yield return Bundles[cur].Execute(info);
        //if (!Bundles[cur].IsValid(info))
        //{
        //    cur++;
        //}
    }

    private bool CheckValid(CombatProcessorInfo info)
    {
        return isValid;
        //foreach (I_CombatProcessor processor in Bundles)
        //{
        //    if (processor.IsValid(info))
        //    {
        //        return true;
        //    }
        //}
        //return false;
    }

    public bool IsFinished(CombatProcessorInfo info)
    {
        return isFinished;
        //foreach (I_CombatProcessor actionBundle in Bundles)
        //{
        //    if (!actionBundle.IsFinished(info))
        //    {
        //        return false;
        //    }
        //}
        //return true;
    }

    public bool IsValid(CombatProcessorInfo info)
    {
        return isValid;
    }
}

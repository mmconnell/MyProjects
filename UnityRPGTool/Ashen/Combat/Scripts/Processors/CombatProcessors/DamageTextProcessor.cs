using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DamageTextProcessor : I_CombatProcessor
{
    public int amount;
    public Transform location;
    public GameObject parent;
    public GameObject damageTextPrefab;

    private bool isValid = true;
    private PoolableDamageText dtPool;

    public IEnumerator Execute(CombatProcessorInfo info)
    {
        dtPool = PoolManager.Instance.GetPoolManager(damageTextPrefab, parent).GetObject() as PoolableDamageText;
        dtPool.mover.tween.Rewind();
        dtPool.fader.tween.Rewind();
        dtPool.transform.position = location.position;
        dtPool.transform.localScale = Vector3.Scale(dtPool.transform.localScale, location.lossyScale);
        dtPool.text.text = "" + amount;
        yield return null;
        dtPool.mover.tween.Play();
        dtPool.fader.tween.Play();
        isValid = false;
        yield break;
    }

    public bool IsFinished(CombatProcessorInfo info)
    {
        bool isFinished = !dtPool.mover.tween.IsPlaying() && !dtPool.fader.tween.IsPlaying();
        if (isFinished)
        {
            dtPool.Disable();
        }
        return isFinished;
    }

    public bool IsValid(CombatProcessorInfo info)
    {
        return isValid;
    }
}
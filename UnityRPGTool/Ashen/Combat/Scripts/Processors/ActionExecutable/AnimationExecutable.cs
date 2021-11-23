﻿using UnityEngine;
using System.Collections;
using Manager;

public class AnimationExecutable : I_ActionExecutable, I_AnimationDisabledListener
{
    public GameObject animation;
    public Vector3 location;
    public float waitTime;
    public PartyPosition position;

    private bool isFinished;

    public void AnimationEndEvent()
    {
        isFinished = true;
    }

    public IEnumerator Execute(MonoBehaviour runner)
    {
        PrefabPool pool = PoolManager.Instance.GetPoolManager(animation);
        PoolableAnimation behaviour = pool.GetObject() as PoolableAnimation;
        SpriteRenderer renderer = behaviour.spriteRenderer;
        renderer.sortingOrder = position.row == PartyRow.FRONT ? 7 : 2;
        behaviour.MovePosition(location);
        behaviour.RegisterAnimationDisabledEvent(this);
        yield return new WaitForSeconds(waitTime);
    }

    public bool IsFinished()
    {
        return isFinished;
    }
}

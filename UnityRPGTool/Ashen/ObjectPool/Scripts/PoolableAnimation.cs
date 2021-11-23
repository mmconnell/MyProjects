using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;

public class PoolableAnimation : PoolableBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public string animationName;
    public GameObject mover;

    private List<I_AnimationDisabledListener> animationDisabledEvents;

    private void Start()
    {
        if (animationName == null || animationName.Length == 0) {
            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
            animationName = controller.layers[0].stateMachine.states[0].state.name;
        }
    }

    public override void Initialize()
    {
        gameObject.SetActive(true);
        enabled = true;
        animator.Play(animationName, -1, 0f);
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
        enabled = false;
        if (animationDisabledEvents != null)
        {
            foreach (I_AnimationDisabledListener listener in animationDisabledEvents)
            {
                listener.AnimationEndEvent();
            }
            animationDisabledEvents.Clear();
        }
    }

    public override bool Enabled()
    {
        return enabled;
    }

    public void RegisterAnimationDisabledEvent(I_AnimationDisabledListener listener)
    {
        if (animationDisabledEvents == null)
        {
            animationDisabledEvents = new List<I_AnimationDisabledListener>();
        }
        animationDisabledEvents.Add(listener);
    }

    public void MovePosition(Vector3 position)
    {
        if (!mover)
        {
            mover = gameObject;
        }
        mover.transform.position = position;
    }
}

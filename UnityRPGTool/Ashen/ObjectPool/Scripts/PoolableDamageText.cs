using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using DG.Tweening;
using TMPro;

public class PoolableDamageText : PoolableBehaviour
{
    public DOTweenAnimation fader;
    public DOTweenAnimation mover;
    public TextMeshProUGUI text;

    private float defaultScaleX;
    private float defaultScaleY;

    public void Awake()
    {
        defaultScaleX = transform.localScale.x;
        defaultScaleY = transform.localScale.y;
    }

    public override void Initialize()
    {
        transform.localScale = new Vector3(defaultScaleX, defaultScaleY, 1f);
        gameObject.SetActive(true);
        enabled = true;
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
        enabled = false;
    }

    public override bool Enabled()
    {
        return enabled;
    }
}

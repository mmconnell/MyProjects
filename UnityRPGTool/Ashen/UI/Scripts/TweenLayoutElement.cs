using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class TweenLayoutElement : MonoBehaviour
{
    Tweener expand;
    Tweener retract;

    LayoutElement layoutElement;

    private void OnEnable()
    {
        layoutElement = GetComponent<LayoutElement>();
        
        
    }

    [Button]
    public void Play()
    {
        expand = DOTween.To(() => layoutElement.preferredWidth, x => layoutElement.preferredWidth = x, 0f, .2f);
    }

    [Button]
    public void Rewind()
    {
        retract = DOTween.To(() => layoutElement.preferredWidth, x => layoutElement.preferredWidth = x, 1000f, .2f);
    }
}

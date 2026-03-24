using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "NewResizeAnimation", menuName = "DOTween/Resize")]
public class ResizeTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    [Header("Tween Settings")] 
    public float duration = 0.5f;
    public Ease easeIn = Ease.OutBack;
    public Ease easeOut = Ease.InBack;
    
    [Header("Target Sizes")]
    public Vector2 expandedSize;
    public Vector2 collapsedSize;

    private RectTransform _defaultTarget;
    
    public void Expand(RectTransform target = null)
    {
        RectTransform rectTransform = target ? target : _defaultTarget;

        if (!rectTransform)
        {
            Debug.LogWarning("No RectTransform provided for Expand!");
            return;
        }

        rectTransform.DOSizeDelta(expandedSize, duration).SetEase(easeIn);
    }

    public void Collapse(RectTransform target = null)
    {
        RectTransform rectTransform = target ? target : _defaultTarget;

        if (!rectTransform)
        {
            Debug.LogWarning("No RectTransform provided for Collapse!");
            return;
        }

        rectTransform.DOSizeDelta(collapsedSize, duration).SetEase(easeOut);
    }

    // Implement abstract TweenAnimationScriptable
    public override void Play(RectTransform target)
    {
        Expand(target);
    }

    public override void Stop(RectTransform target)
    {
        Collapse(target);
    }
}

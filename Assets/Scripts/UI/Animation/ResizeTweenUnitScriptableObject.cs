using UnityEngine;
using DG.Tweening;
using System;

[CreateAssetMenu(fileName = "NewResizeAnimation", menuName = "DOTween/Resize")]
public class ResizeTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    [Header("Tween Settings")]
    public Ease easeIn = Ease.OutBack;
    public Ease easeOut = Ease.InBack;
    
    [Header("Target Sizes")]
    public Vector2 expandedSize;
    public Vector2 collapsedSize;

    private void Expand(RectTransform target, Action onComplete = null)
    {
        if (!target) return;
        target.DOSizeDelta(expandedSize, duration).SetEase(easeIn).OnComplete(() => onComplete?.Invoke());
    }

    private void Collapse(RectTransform target, Action onComplete = null)
    {
        if (!target) return;
        target.DOSizeDelta(collapsedSize, duration).SetEase(easeOut).OnComplete(() => onComplete?.Invoke());
    }

    public override void Play(RectTransform rectTransform, Action onComplete = null) => Expand(rectTransform, onComplete);
    public override void PlayReverse(RectTransform rectTransform, Action onComplete = null) => Collapse(rectTransform, onComplete);
    public override void Stop(RectTransform rectTransform) => Collapse(rectTransform);
}

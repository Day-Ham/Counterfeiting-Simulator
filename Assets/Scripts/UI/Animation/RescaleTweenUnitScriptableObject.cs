using System;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "NewRescaleAnimation", menuName = "DOTween/Rescale")]
public class RescaleTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    [Header("Tween Settings")]
    public Ease easeIn = Ease.OutBack;
    public Ease easeOut = Ease.InBack;

    [Header("Target Scales")]
    public Vector3 expandedScale;
    public Vector3 collapsedScale;

    private void Expand(RectTransform target, Action onComplete = null)
    {
        if (!target) return;
        target.DOScale(expandedScale, duration).SetEase(easeIn).OnComplete(() => onComplete?.Invoke());
    }

    private void Collapse(RectTransform target, Action onComplete = null)
    {
        if (!target) return;
        target.DOScale(collapsedScale, duration).SetEase(easeOut).OnComplete(() => onComplete?.Invoke());
    }

    public override void Play(RectTransform rectTransform, Action onComplete = null) => Expand(rectTransform, onComplete);
    public override void PlayReverse(RectTransform rectTransform, Action onComplete = null) => Collapse(rectTransform, onComplete);
    public override void Stop(RectTransform rectTransform) => Collapse(rectTransform);

}
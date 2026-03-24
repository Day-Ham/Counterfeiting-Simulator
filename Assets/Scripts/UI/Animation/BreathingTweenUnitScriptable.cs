using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBreathingAnimation", menuName = "DOTween/Breath")]
public class BreathingTweenUnitScriptable : TweenAnimationUnitScriptable
{
    [Header("Size Settings")]
    public Vector2 smallSize;
    public Vector2 largeSize;
    
    [Header("Time Settings")]
    public float duration;
    
    [Header("Easing Settings")]
    public Ease ease = Ease.InOutSine;

    private Tween _activeTween;

    public override void Play(RectTransform target)
    {
        if (!target) return;
        _activeTween?.Kill();
        target.sizeDelta = smallSize;
        _activeTween = target.DOSizeDelta(largeSize, duration)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void Stop(RectTransform target)
    {
        _activeTween?.Kill();
        _activeTween = null;
    }
}

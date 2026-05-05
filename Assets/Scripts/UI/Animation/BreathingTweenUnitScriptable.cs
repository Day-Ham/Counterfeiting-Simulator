using DG.Tweening;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewBreathingAnimation", menuName = "DOTween/Breath")]
public class BreathingTweenUnitScriptable : TweenAnimationUnitScriptable
{
    [Header("Easing Settings")]
    public Ease ease = Ease.InOutSine;
    
    [Header("Size Settings")]
    public Vector2 smallSize;
    public Vector2 largeSize;
    
    [Header("Behavior")]
    public bool isPlayIntroFirst;

    private Tween _activeTween;

    public override void Play(RectTransform rectTransform, Action onComplete = null)
    {
        if (!rectTransform) return;

        _activeTween?.Kill();

        // Case 1: Start from 0 and zoom to the smallSize
        if (isPlayIntroFirst)
        {
            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(rectTransform.DOSizeDelta(smallSize, duration * 0.5f).SetEase(ease))
                .OnComplete(() => StartBreathingLoop(rectTransform));

            _activeTween = sequence;
        }
        else
        {
            // Case 2: UI is already visible
            StartBreathingLoop(rectTransform);
        }
    }

    private void StartBreathingLoop(RectTransform rectTransform)
    {
        rectTransform.sizeDelta = smallSize;
        
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(rectTransform.DOSizeDelta(largeSize, duration).SetEase(ease))
            .Append(rectTransform.DOSizeDelta(smallSize, duration).SetEase(ease))
            .SetLoops(-1);

        _activeTween = sequence;
    }

    public override void Stop(RectTransform rectTransform)
    {
        _activeTween?.Kill();
        _activeTween = null;
    }
}

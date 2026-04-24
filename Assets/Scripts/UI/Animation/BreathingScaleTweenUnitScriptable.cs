using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBreathingScaleAnimation", menuName = "DOTween/Breath Scale")]
public class BreathingScaleTweenUnitScriptable : TweenAnimationUnitScriptable
{
    [Header("Easing Settings")]
    public Ease ease = Ease.InOutSine;

    [Header("Scale Settings")]
    public Vector3 smallScale;
    public Vector3 largeScale;

    [Header("Behavior")]
    public bool isPlayIntroFirst;

    private Tween _activeTween;

    public override void Play(RectTransform rectTransform)
    {
        if (!rectTransform) return;

        _activeTween?.Kill();

        if (isPlayIntroFirst)
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(rectTransform.DOScale(smallScale, duration * 0.5f).SetEase(ease))
                .OnComplete(() => StartBreathingLoop(rectTransform));
            _activeTween = sequence;
        }
        else
        {
            StartBreathingLoop(rectTransform);
        }
    }

    private void StartBreathingLoop(RectTransform rectTransform)
    {
        rectTransform.localScale = smallScale;

        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(rectTransform.DOScale(largeScale, duration).SetEase(ease))
            .Append(rectTransform.DOScale(smallScale, duration).SetEase(ease))
            .SetLoops(-1);

        _activeTween = sequence;
    }

    public override void Stop(RectTransform rectTransform)
    {
        _activeTween?.Kill();
        _activeTween = null;
    }
}
using UnityEngine;
using DG.Tweening;
using System;

[CreateAssetMenu(fileName = "NewFadeAnimation", menuName = "DOTween/Fade")]
public class FadeTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    [Header("Tween Settings")]
    public Ease easeIn = Ease.OutQuad;
    public Ease easeOut = Ease.InQuad;

    [Header("Target Alphas")]
    public float fadeInAlpha = 1f;
    public float fadeOutAlpha = 0f;

    private void FadeIn(RectTransform target, Action onComplete = null)
    {
        if (!target) return;
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (!canvasGroup) return;
        canvasGroup.DOKill();
        canvasGroup.alpha = fadeOutAlpha;
        canvasGroup.DOFade(fadeInAlpha, duration).SetEase(easeIn).OnComplete(() => onComplete?.Invoke());
    }

    private void FadeOut(RectTransform target, Action onComplete = null)
    {
        if (!target) return;
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (!canvasGroup) return;
        canvasGroup.DOFade(fadeOutAlpha, duration).SetEase(easeOut).OnComplete(() => onComplete?.Invoke());
    }

    public override void Play(RectTransform rectTransform, Action onComplete = null) => FadeIn(rectTransform, onComplete);
    public override void PlayReverse(RectTransform rectTransform, Action onComplete = null) => FadeOut(rectTransform, onComplete);
}

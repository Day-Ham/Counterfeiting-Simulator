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

    public void FadeIn(RectTransform target, Action onComplete)
    {
        if (!target) return;
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (!canvasGroup) return;
        canvasGroup.DOKill();
        canvasGroup.alpha = fadeOutAlpha;
        canvasGroup.DOFade(fadeInAlpha, duration).SetEase(easeIn).OnComplete(() => onComplete?.Invoke());
    }

    public void FadeOut(RectTransform target, Action onComplete)
    {
        if (!target) return;
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (!canvasGroup) 
        {
            Debug.LogWarning("Target does not have a CanvasGroup component.");
            return;
        }
        canvasGroup.DOFade(fadeOutAlpha, duration).SetEase(easeOut).OnComplete(() => onComplete?.Invoke());
    }

    public override void Play(RectTransform rectTransform)
    {
        FadeIn(rectTransform, null);
    }

    public override void Play(RectTransform rectTransform, Action onComplete)
    {
        FadeIn(rectTransform, onComplete);
    }

    public override void PlayReverse(RectTransform rectTransform, Action onComplete)
    {
        FadeOut(rectTransform, onComplete);
    }
}

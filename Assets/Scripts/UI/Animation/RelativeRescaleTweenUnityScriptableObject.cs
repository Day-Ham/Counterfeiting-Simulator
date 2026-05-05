using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "DOTween/Relative Rescale")]
public class RelativeRescaleTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    public Ease easeIn = Ease.OutBack;
    public Ease easeOut = Ease.InBack;
    public float expandMultiplier = 1.2f;

    public void Expand(RectTransform target, Vector3 originalScale)
    {
        if (!target) return;
        target.DOScale(originalScale * expandMultiplier, duration).SetEase(easeIn);
    }

    public void Collapse(RectTransform target, Vector3 originalScale)
    {
        if (!target) return;
        target.DOScale(originalScale, duration).SetEase(easeOut);
    }
}

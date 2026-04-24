using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "NewMoveAnimation", menuName = "DOTween/Move")]
public class MoveTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    [Header("Tween Settings")]
    public Ease ease = Ease.OutBack;

    [Header("Target Positions")]
    public Vector2 fromPosition;
    public Vector2 toPosition;

    public override void Play(RectTransform rectTransform)
    {
        if (!rectTransform) return;
        rectTransform.DOAnchorPos(toPosition, duration).SetEase(ease);
    }

    public void PlayReverse(RectTransform rectTransform)
    {
        if (!rectTransform) return;
        rectTransform.DOAnchorPos(fromPosition, duration).SetEase(ease);
    }

    public override void Stop(RectTransform rectTransform)
    {
        rectTransform?.DOKill();
    }
}

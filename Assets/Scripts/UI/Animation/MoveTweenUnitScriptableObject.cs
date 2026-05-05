using UnityEngine;
using DG.Tweening;
using System;

[CreateAssetMenu(fileName = "NewMoveAnimation", menuName = "DOTween/Move")]
public class MoveTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    [Header("Tween Settings")]
    public Ease ease = Ease.OutBack;

    [Header("Target Positions")]
    public Vector2 fromPosition;
    public Vector2 toPosition;

    public override void Play(RectTransform rectTransform, Action onComplete = null)
    {
        if (!rectTransform) return;
        rectTransform.DOAnchorPos(toPosition, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }

    public override void PlayReverse(RectTransform rectTransform, Action onComplete = null)
    {
        if (!rectTransform) return;
        rectTransform.DOAnchorPos(fromPosition, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }

    public override void Stop(RectTransform rectTransform) => rectTransform?.DOKill();
}

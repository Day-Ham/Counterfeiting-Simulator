using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "NewResizeAnimation", menuName = "DOTween/Resize")]
public class ResizeTweenUnitScriptableObject : TweenAnimationUnitScriptable
{
    [Header("Tween Settings")]
    public Ease easeIn = Ease.OutBack;
    public Ease easeOut = Ease.InBack;
    
    [Header("Target Sizes")]
    public Vector2 expandedSize;
    public Vector2 collapsedSize;

    private RectTransform _defaultTarget;
    
    public void Expand(RectTransform target = null)
    {
        RectTransform rectTransform = target ? target : _defaultTarget;

        if (!rectTransform)
        {
            Debug.LogWarning("No RectTransform provided for Expand!");
            return;
        }

        rectTransform.DOSizeDelta(expandedSize, duration).SetEase(easeIn);
    }

    public void Collapse(RectTransform target = null)
    {
        RectTransform rectTransform = target ? target : _defaultTarget;

        if (!rectTransform)
        {
            Debug.LogWarning("No RectTransform provided for Collapse!");
            return;
        }

        rectTransform.DOSizeDelta(collapsedSize, duration).SetEase(easeOut);
    }
    
    public override void Play(RectTransform rectTransform)
    {
        Expand(rectTransform);
    }

    public override void Stop(RectTransform rectTransform)
    {
        Collapse(rectTransform);
    }
}

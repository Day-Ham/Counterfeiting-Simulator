using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Resize Animation", menuName = "Settings/GameObject/Resize")]
public class ResizeTweenScriptableObject : ScriptableObject
{
    private RectTransform _defaultTarget;

    [Header("Tween Settings")] 
    public float duration;
    public Ease easeIn;
    public Ease easeOut;
    
    [Header("Target Sizes")]
    public Vector2 expandedSize;
    public Vector2 collapsedSize;
    
    public void Expand(RectTransform target = null)
    {
        RectTransform rectTransform;

        if (target)
        {
            rectTransform = target;
        }
        else
        {
            rectTransform = _defaultTarget;
        }

        if (!rectTransform)
        {
            Debug.LogWarning("No RectTransform provided!");
            return;
        }

        rectTransform.DOSizeDelta(expandedSize, duration).SetEase(easeIn);
    }
    
    public void Collapse(RectTransform target = null)
    {
        RectTransform rectTransform;

        if (target)
        {
            rectTransform = target;
        }
        else
        {
            rectTransform = _defaultTarget;
        }

        if (!rectTransform)
        {
            Debug.LogWarning("No RectTransform provided!");
            return;
        }

        rectTransform.DOSizeDelta(collapsedSize, duration).SetEase(easeOut);
    }
}

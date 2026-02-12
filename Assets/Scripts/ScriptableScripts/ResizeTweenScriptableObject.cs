using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Resize Animation", menuName = "Settings/GameObject/Resize")]
public class ResizeTweenScriptableObject : ScriptableObject
{
    public GameObjectValue GameObject;

    [Header("Tween Settings")] 
    public float Duration;
    public Ease EaseIn;
    public Ease EaseOut;
    
    [Header("Target Sizes")]
    public Vector2 ExpandedSize;
    public Vector2 CollapsedSize;
    
    public void Expand(GameObject target = null)
    {
        RectTransform rectTransform = GetRectTransform(target);
        if (!rectTransform) return;

        rectTransform.DOSizeDelta(ExpandedSize, Duration).SetEase(EaseIn);
    }
    
    public void Collapse(GameObject target = null)
    {
        RectTransform rectTransform = GetRectTransform(target);
        
        if (!rectTransform) return;

        rectTransform.DOSizeDelta(CollapsedSize, Duration).SetEase(EaseOut);
    }
    
    private RectTransform GetRectTransform(GameObject target)
    {
        GameObject gameObject = target;

        if (!gameObject)
        {
            if (!GameObject || !GameObject.Value)
            {
                Debug.LogWarning("No GameObject provided for resizing!");
                return null;
            }

            gameObject = GameObject.Value;
        }

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        if (rectTransform) return rectTransform;
        
        Debug.LogWarning("Target does not have a RectTransform!");
        
        return null;

    }
}

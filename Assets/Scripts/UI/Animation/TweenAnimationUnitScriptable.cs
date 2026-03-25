using UnityEngine;

public abstract class TweenAnimationUnitScriptable : ScriptableObject
{
    [Header("Time Settings")]
    public float duration;
    
    public abstract void Play(RectTransform rectTransform);
    public virtual void Stop(RectTransform rectTransform) { }
}

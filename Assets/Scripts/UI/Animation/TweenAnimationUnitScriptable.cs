using System;
using UnityEngine;

public abstract class TweenAnimationUnitScriptable : ScriptableObject
{
    [Header("Time Settings")]
    public float duration;
    
    public virtual void Play(RectTransform rectTransform) { }
    public virtual void Stop(RectTransform rectTransform) { }
    public virtual void PlayReverse(RectTransform rectTransform) { }
    public virtual void Play(RectTransform rectTransform, Action onComplete = null) => Play(rectTransform);
    public virtual void PlayReverse(RectTransform rectTransform, Action onComplete = null) => PlayReverse(rectTransform);
}

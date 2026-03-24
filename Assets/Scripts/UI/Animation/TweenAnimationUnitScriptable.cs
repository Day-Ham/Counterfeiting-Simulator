using UnityEngine;

public abstract class TweenAnimationUnitScriptable : ScriptableObject
{
    public abstract void Play(RectTransform target);
    public virtual void Stop(RectTransform target) { }
}

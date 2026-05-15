using UnityEngine;

public abstract class TimerThreshold : ScriptableObject
{
    public VoidEvent onThresholdReachedEvent;
    public string notificationMessage;

    public abstract bool ShouldNotify(float remainingTime);
    public abstract bool ShouldCountdown(float remainingTime);
    public abstract bool ShouldFire(float remainingTime);
    public abstract void Reset(float totalTime);
}

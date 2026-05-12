using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "OneShotTimerThreshold", menuName = "Timer/OneShotTimerThreshold")]
public class OneShotTimerThreshold : TimerThreshold
{
    [Header("Threshold Settings")]
    public float thresholdTime;

    [HideInInspector] public bool hasFired;
    [HideInInspector] public bool hasNotified;
    [HideInInspector] public bool hasCountdown;

    private float NotificationTime => thresholdTime + 10f;
    private float CountdownTime => thresholdTime + 5f;

    public override bool ShouldNotify(float remainingTime)
    {
        if (hasNotified || remainingTime > NotificationTime) return false;
        hasNotified = true;
        return true;
    }

    public override bool ShouldCountdown(float remainingTime)
    {
        if (hasCountdown || remainingTime > CountdownTime) return false;
        hasCountdown = true;
        return true;
    }

    public override bool ShouldFire(float remainingTime)
    {
        if (hasFired || remainingTime > thresholdTime) return false;
        hasFired = true;
        return true;
    }

    public override void Reset(float totalTime)
    {
        hasFired = false;
        hasNotified = false;
        hasCountdown = false;
    }
}

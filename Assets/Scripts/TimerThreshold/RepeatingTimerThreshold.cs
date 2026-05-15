using UnityEngine;

[CreateAssetMenu(fileName = "RepeatingTimerThreshold", menuName = "Timer/RepeatingTimerThreshold")]
public class RepeatingTimerThreshold : TimerThreshold
{
    public float interval = 60f;

    [HideInInspector] public float nextFireTime;
    private bool _hasNotified;
    private bool _hasCountdown;

    public override bool ShouldNotify(float remainingTime)
    {
        if (_hasNotified || remainingTime > nextFireTime + 10f) return false;
        _hasNotified = true;
        return true;
    }

    public override bool ShouldCountdown(float remainingTime)
    {
        if (_hasCountdown || remainingTime > nextFireTime + 5f) return false;
        _hasCountdown = true;
        return true;
    }

    public override bool ShouldFire(float remainingTime)
    {
        if (remainingTime > nextFireTime) return false;
        nextFireTime = remainingTime - interval;
        _hasNotified = false;
        _hasCountdown = false;
        return true;
    }

    public override void Reset(float totalTime)
    {
        nextFireTime = totalTime - interval;
        _hasNotified = false;
        _hasCountdown = false;
    }
}

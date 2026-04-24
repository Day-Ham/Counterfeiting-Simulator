using UnityEngine;

[CreateAssetMenu(fileName = "TimerThreshold", menuName = "Timer/TimerThreshold")]
public class TimerThreshold : ScriptableObject
{
    [Header("Threshold Settings")]
    public float thresholdTime;
    public VoidEvent onThresholdReachedEvent;

    [HideInInspector] public bool hasFired;
}

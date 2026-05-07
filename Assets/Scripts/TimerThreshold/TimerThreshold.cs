using UnityEngine;

[CreateAssetMenu(fileName = "TimerThreshold", menuName = "Timer/TimerThreshold")]
public class TimerThreshold : ScriptableObject
{
    [Header("Threshold Settings")]
    public float thresholdTime;
    public VoidEvent onThresholdReachedEvent;

    [Header("Notification Settings")]
    public string notificationMessage;
    [HideInInspector] public bool hasFired;
    [HideInInspector] public bool hasNotified;
    [HideInInspector] public bool hasCountdown;

    public float NotificationTime => thresholdTime + 10f; // Notify 10 seconds before the threshold
    public float CountdownTime => thresholdTime + 5f; // Start countdown 5 seconds before the threshold  
}

using System;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private FloatEvent timerTickEvent;
    [SerializeField] private VoidEvent startInspectionEvent;
    [SerializeField] private FloatEvent timerStartEvent;
    [SerializeField] private StringEvent onNotificationShowEvent;
    [SerializeField] private VoidEvent onCountdownStartEvent;
    [SerializeField] private VoidEvent onNotificationHideEvent;

    [Header("Settings")]
    [SerializeField] private List<TimerThreshold> _timerThresholds;

    [Header("Runtime")]
    [SerializeField] private MainGameConfigRuntimeAsset runtimeAsset;
    private float _remainingTime;
    private bool _isRunning;

    private void OnEnable()
    {
        GameState.OnGameStarted += StartTimer;
        GameState.OnGameFinished += StopTimer;
        GameState.OnGamePaused += StopTimer;
        GameState.OnGameResumed += ResumeTimer;
    }

    private void OnDisable()
    {
        GameState.OnGameStarted -= StartTimer;
        GameState.OnGameFinished -= StopTimer;
        GameState.OnGamePaused -= StopTimer;
        GameState.OnGameResumed -= ResumeTimer;
    }

    private void Awake()
    {
        if (runtimeAsset != null)
        {
            _timerThresholds = runtimeAsset.Value?.TimerThresholds?.Value;
        }
    }

    private void StartTimer()
    {
        if (runtimeAsset == null || !runtimeAsset.HasValue) return;
        if (runtimeAsset.Value.TimeLimit == null) return;

        _remainingTime = runtimeAsset.Value.TimeLimit.Value;
        _isRunning = true;

        if(_timerThresholds != null)
        {
            foreach (var threshold in _timerThresholds)
            {
                threshold.hasFired = false;
                threshold.hasNotified = false;
                threshold.hasCountdown = false;
            }
        }

        timerStartEvent.Raise(_remainingTime);
    }

    private void StopTimer() => _isRunning = false;
    private void ResumeTimer() => _isRunning = true;

    private void Update()
    {
        if (!_isRunning) return;

        _remainingTime = Mathf.Max(0f, _remainingTime - Time.deltaTime);
        timerTickEvent.Raise(_remainingTime);

        if(_timerThresholds != null)
        {
            foreach (var threshold in _timerThresholds)
            {
                if (!threshold.hasNotified && _remainingTime <= threshold.NotificationTime)
                {
                    threshold.hasNotified = true;
                    onNotificationShowEvent.Raise(threshold.notificationMessage);
                }

                 if (!threshold.hasCountdown && _remainingTime <= threshold.CountdownTime)
                {
                    threshold.hasCountdown = true;
                    onCountdownStartEvent.Raise();
                }
                if (!threshold.hasFired && _remainingTime <= threshold.thresholdTime)
                {
                    threshold.hasFired = true;
                    threshold.onThresholdReachedEvent.Raise();
                    onNotificationHideEvent.Raise();
                }
            }
        }

        if (_remainingTime <= 0)
        {
            _remainingTime = 0;
            _isRunning = false;
            startInspectionEvent.Raise();
        }

        if (Mathf.FloorToInt(_remainingTime) != Mathf.FloorToInt(_remainingTime + Time.deltaTime))
        {
            //Debug.Log($"[Timer] {_remainingTime:F1}s remaining");
        }
    }
}

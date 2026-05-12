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
                threshold.Reset(_remainingTime);
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
                if (threshold.ShouldNotify(_remainingTime))
                {
                    Debug.Log($"[Timer] Threshold reached: {threshold.notificationMessage}");
                    onNotificationShowEvent.Raise(threshold.notificationMessage);
                }
                if (threshold.ShouldCountdown(_remainingTime))
                {
                    Debug.Log($"[Timer] Countdown started: {threshold.notificationMessage}");
                    onCountdownStartEvent.Raise();
                }
                if (threshold.ShouldFire(_remainingTime))
                {
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

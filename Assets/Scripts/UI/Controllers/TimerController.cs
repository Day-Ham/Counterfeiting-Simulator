using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private FloatEvent timerTickEvent;
    [SerializeField] private VoidEvent startInspectionEvent;
    [SerializeField] private FloatEvent timerStartEvent;

    [Header("Settings")]
    [SerializeField] private List<TimerThreshold> timerThresholds;

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

    private void StartTimer()
    {
        if (runtimeAsset == null || !runtimeAsset.HasValue) return;
        if (runtimeAsset.Value.TimeLimit == null) return;

        _remainingTime = runtimeAsset.Value.TimeLimit.Value;
        _isRunning = true;

        foreach (var threshold in timerThresholds)
        {
            threshold.hasFired = false;
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

        foreach (var threshold in timerThresholds)
        {
            if (!threshold.hasFired && _remainingTime <= threshold.thresholdTime)
            {
                threshold.hasFired = true;
                threshold.onThresholdReachedEvent.Raise();
            }
        }

        if (_remainingTime <= 0)
        {
            _remainingTime = 0;
            _isRunning = false;
            startInspectionEvent.Raise();
        }
    }
}

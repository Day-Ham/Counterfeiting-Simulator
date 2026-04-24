using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private FloatEvent timerTickEvent;
    [SerializeField] private FloatEvent timerStartEvent;

    [Header("UI")]
    [SerializeField] private Image radialFill;

    private float _totalDuration;

    private void OnEnable()
    {
        timerTickEvent.Register(OnTimerTick);
        timerStartEvent.Register(OnTimerStart);
    }

    private void OnDisable()
    {
        timerTickEvent.Unregister(OnTimerTick);
        timerStartEvent.Unregister(OnTimerStart);
    }

    private void OnTimerStart(float totalDuration)
    {
        _totalDuration = totalDuration;
    }

    private void OnTimerTick(float remainingTime)
    {
        radialFill.fillAmount = remainingTime / _totalDuration;
    }
}

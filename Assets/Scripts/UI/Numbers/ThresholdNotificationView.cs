using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class ThresholdNotificationView : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private StringEvent onNotificationShowEvent;
    [SerializeField] private VoidEvent onCountdownStartEvent;
    [SerializeField] private VoidEvent onNotificationHideEvent;
    [SerializeField] private AudioClipEvent playSFXEvent;

    [Header("Tween")]
    [SerializeField] private TweenAnimationUnitScriptable popUpTween;
    [SerializeField] private BreathingScaleTweenUnitScriptable breathingTween;

    [Header("UI")]
    [SerializeField] private RectTransform target;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("SFX")]
    [SerializeField] private AudioClipValue notificationShowClip;
    [SerializeField] private AudioClipValue notificationHideClip;
    [SerializeField] private AudioClipValue countdownTickClip;

    private Coroutine _countdownCoroutine;

    private void OnEnable()
    {
        onNotificationShowEvent.Register(Show);
        onCountdownStartEvent.Register(StartCountdown);
        onNotificationHideEvent.Register(Hide);
    }

    private void OnDisable()
    {
        onNotificationShowEvent.Unregister(Show);
        onCountdownStartEvent.Unregister(StartCountdown);
        onNotificationHideEvent.Unregister(Hide);
    }

    private void Awake()
    {
        target.localScale = Vector3.zero;
        messageText.SetText("");
    }

    private void Show(string message)
    {
        messageText.SetText(message);
        popUpTween.Play(target, () =>
        {
            breathingTween.Play(target);
        });
        playSFXEvent.Raise(notificationShowClip.Value);
    }

    private void StartCountdown()
    {
        if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);
        playSFXEvent.Raise(countdownTickClip.Value);
    }

    private void Hide()
    {
        if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);
        breathingTween.Stop(target);
        popUpTween.PlayReverse(target, () =>
        {
            messageText.SetText("");
        });
        playSFXEvent.Raise(notificationHideClip.Value);
    }
}

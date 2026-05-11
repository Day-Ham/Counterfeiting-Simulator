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

    [Header("UI")]
    [SerializeField] private RectTransform target;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI countdownText;

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
        countdownText.SetText("");
        messageText.SetText("");
    }

    private void Show(string message)
    {
        messageText.SetText(message);
        countdownText.SetText("");
        popUpTween.Play(target);
        playSFXEvent.Raise(notificationShowClip.Value);
    }

    private void StartCountdown()
    {
        if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);
        playSFXEvent.Raise(countdownTickClip.Value);
        _countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        for (int i = 5; i > 0; i--)
        {
            countdownText.SetText(i.ToString());
            countdownText.transform.DOPunchScale(Vector3.one * 0.3f, 0.4f, vibrato: 5);
            yield return new WaitForSeconds(1f);
        }
    }

    private void Hide()
    {
        if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);
        popUpTween.PlayReverse(target, () =>
        {
            messageText.SetText("");
            countdownText.SetText("");
        });
        playSFXEvent.Raise(notificationHideClip.Value);
    }
}

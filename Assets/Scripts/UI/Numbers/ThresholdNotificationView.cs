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

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI countdownText;

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
        canvasGroup.alpha = 0f;
        countdownText.SetText("");
        messageText.SetText("");
    }

    private void Show(string message)
    {
        messageText.SetText(message);
        countdownText.SetText("");
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.3f);
    }

    private void StartCountdown()
    {
        if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);
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
        canvasGroup.DOFade(0f, 0.3f);
    }
}

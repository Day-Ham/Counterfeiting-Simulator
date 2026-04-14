using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToLatestHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentParent;

    [Header("Tween Settings")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    [Header("Events")]
    [SerializeField] private VoidEvent autoScrollEvent;

    [Header("Auto Trigger")]
    [SerializeField] private bool autoScrollOnStart = false;

    [Header("Debug")]
    [SerializeField] private bool debugAutoScroll;

    private Tween _tween;
    private Coroutine _coroutine;

    private bool IsHorizontal => scrollRect.horizontal;

    private void OnEnable()
    {
        autoScrollEvent.Register(TriggerScroll);
    }

    private void OnDisable()
    {
        autoScrollEvent.Unregister(TriggerScroll);
    }

    private void Start()
    {
        if (autoScrollOnStart) TriggerScroll();
    }

    private void Update()
    {
        HandleDebugTrigger();
    }
    
    private void TriggerScroll()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(ScrollRoutine());
    }
    
    private IEnumerator ScrollRoutine()
    {
        SetInteractable(false);

        yield return null;
        yield return new WaitForEndOfFrame();

        RebuildLayout();
        KillTween();

        float target = GetTarget();

        _tween = DOTween
            .To(GetPosition, SetPosition, target, duration)
            .SetEase(ease);

        yield return _tween.WaitForCompletion();

        SetInteractable(true);
    }
    
    private void HandleDebugTrigger()
    {
        if (!debugAutoScroll) return;

        debugAutoScroll = false;
        TriggerScroll();
    }

    [ContextMenu("DEBUG Scroll To Latest")]
    private void DebugScroll()
    {
        TriggerScroll();
    }
    
    private float GetPosition()
    {
        return IsHorizontal
            ? scrollRect.horizontalNormalizedPosition
            : scrollRect.verticalNormalizedPosition;
    }

    private void SetPosition(float value)
    {
        if (IsHorizontal)
        {
            scrollRect.horizontalNormalizedPosition = value;
        }
        else
        {
            scrollRect.verticalNormalizedPosition = value;
        }
    }

    private float GetTarget()
    {
        return IsHorizontal ? 1f : 0f;
    }

    private void SetInteractable(bool value)
    {
        scrollRect.enabled = value;
        scrollRect.inertia = value;
    }

    private void RebuildLayout()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
    }

    private void KillTween()
    {
        if (_tween != null && _tween.IsActive())
        {
            _tween.Kill();
        }
    }
}

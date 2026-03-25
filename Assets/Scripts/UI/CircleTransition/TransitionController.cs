using System;
using DG.Tweening;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] private GameObject circleUI;

    [Header("Tween Settings")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.OutQuad;

    [Header("Event")]
    [SerializeField] private CallbackEvent playTransitionEvent;

    private void OnEnable()
    {
        playTransitionEvent?.Register(HandleTransition);
    }

    private void OnDisable()
    {
        playTransitionEvent?.Unregister(HandleTransition);
    }

    private void Start()
    {
        circleUI.SetActive(true);
        circleUI.transform.localScale = Vector3.one * 25f;
        circleUI.transform.DOScale(Vector3.zero, duration).SetEase(ease);
    }

    private void HandleTransition(Action onComplete)
    {
        circleUI.SetActive(true);
        circleUI.transform.localScale = Vector3.zero;
        
        circleUI.transform
            .DOScale(Vector3.one * 25f, duration)
            .SetEase(ease)
            .OnComplete(() => onComplete?.Invoke());
    }
}

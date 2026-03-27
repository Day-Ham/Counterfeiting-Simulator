using System;
using DG.Tweening;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent playTransitionEvent;
    [SerializeField] private VoidEvent onTransitionFinished;
    
    [Header("Transition")]
    [SerializeField] private GameObject circleUI;

    [Header("Tween Settings")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.OutQuad;

    private void OnEnable()
    {
        playTransitionEvent?.Register(PlayTransition);
    }

    private void OnDisable()
    {
        playTransitionEvent?.Unregister(PlayTransition);
    }
    
    private void Start()
    {
        PlayIntroTransition();
    }

    // Intro (scene enter: big -> small)
    private void PlayIntroTransition()
    {
        circleUI.SetActive(true);

        circleUI.transform.localScale = Vector3.one * 25f;

        circleUI.transform
            .DOScale(Vector3.zero, duration)
            .SetEase(ease);
    }

    // Normal Transition (small -> big)
    private void PlayTransition()
    {
        circleUI.SetActive(true);

        circleUI.transform.localScale = Vector3.zero;

        circleUI.transform
            .DOScale(Vector3.one * 25f, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                onTransitionFinished.Raise();
            });
    }
}

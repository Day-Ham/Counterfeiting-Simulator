using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class HintView : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private HintDataEvent hintDataEvent;
    [SerializeField] private VoidEvent hintDismissedEvent;

    [Header("Tweens")]
    [SerializeField] private RescaleTweenUnitScriptableObject arrowRescaleTweenUnitScriptableObject;
    [SerializeField] private RescaleTweenUnitScriptableObject hintBGRescaleTweenUnitScriptableObject;
    [SerializeField] private BreathingScaleTweenUnitScriptable arrowBreathingTweenUnitScriptableObject;
    [SerializeField] private BreathingScaleTweenUnitScriptable hintBGBreathingTweenUnitScriptableObject;

    [Header("UI")]
    [SerializeField] private RectTransform hintParentRectTransform;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private RectTransform arrowRectTransform;

    [Header("UI Settings")]
    [SerializeField] private Vector2 defaultScreenPosition;

    private Coroutine _dismissCoroutine;

    private void OnEnable()
    {
        hintDataEvent.Register(ShowHint);
        GameState.OnGamePaused += PauseAnimations;
        GameState.OnGameResumed += ResumeAnimations;
    }

    private void OnDisable()
    {
        hintDataEvent.Unregister(ShowHint);
        GameState.OnGamePaused -= PauseAnimations;
        GameState.OnGameResumed -= ResumeAnimations;
    }

    private void Start()
    {
        hintParentRectTransform.localScale = Vector3.zero;
        arrowRectTransform.localScale = Vector3.zero;
    }
    
    private void PauseAnimations()
    {
        hintBGBreathingTweenUnitScriptableObject.Pause();
        arrowBreathingTweenUnitScriptableObject.Pause();
    }

    private void ResumeAnimations()
    {
        hintBGBreathingTweenUnitScriptableObject.Resume();
        arrowBreathingTweenUnitScriptableObject.Resume();
    }

    private void ShowHint(HintData hint)
    {
        if (_dismissCoroutine != null)
            StopCoroutine(_dismissCoroutine);

        hintParentRectTransform.anchoredPosition = hint.screenPosition != Vector2.zero ? hint.screenPosition : defaultScreenPosition;
        hintBGRescaleTweenUnitScriptableObject.Expand(hintParentRectTransform, () =>
        {
            hintBGBreathingTweenUnitScriptableObject.Play(hintParentRectTransform);
        });
        hintText.text = hint.message;

        if (hint.useArrow)
        {
            arrowRectTransform.anchoredPosition = hint.arrowPosition;
            arrowRectTransform.localRotation = Quaternion.Euler(0, 0, hint.arrowRotation);
            arrowRescaleTweenUnitScriptableObject.Expand(arrowRectTransform, () =>
            {
                arrowBreathingTweenUnitScriptableObject.Play(arrowRectTransform);
            });
        }
        else
        {
            arrowRescaleTweenUnitScriptableObject.Collapse(arrowRectTransform);
        }

        _dismissCoroutine = StartCoroutine(DismissAfterDelay(hint.displayDuration));
    }

    private IEnumerator DismissAfterDelay(float duration)
    {
        yield return CoroutineUtility.PauseAwareWait(duration);
        arrowRescaleTweenUnitScriptableObject.Collapse(arrowRectTransform, () =>
        {
            arrowBreathingTweenUnitScriptableObject.Stop(arrowRectTransform);
        });
        hintBGRescaleTweenUnitScriptableObject.Collapse(hintParentRectTransform, () =>
        {
            hintBGBreathingTweenUnitScriptableObject.Stop(hintParentRectTransform);
            hintDismissedEvent.Raise();
        });
    }
}

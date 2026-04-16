using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UITransitionElement : MonoBehaviour
{
    [Header("Bindings")]
    [SerializeField] private List<UITransitionBinding> bindings;

    [Header("Events")]
    [SerializeField] private IntEvent batchEvent;
    [SerializeField] private IntEvent registerElementEvent;
    [SerializeField] private VoidEvent elementCompleteEvent;

    [Header("Tween Settings")]
    [SerializeField] private UITransitionTweenSettings tween;

    [Header("Positions")]
    [SerializeField] private RectTransform rectTransform;

    [Tooltip("Where UI appears")]
    [SerializeField] private Vector2 enterPosition;

    [Tooltip("Where UI goes when hidden OR shifted (side/offscreen/etc)")]
    [SerializeField] private Vector2 exitPosition;

    private void OnEnable()
    {
        batchEvent.Register(OnBatchTriggered);
    }

    private void OnDisable()
    {
        batchEvent.Unregister(OnBatchTriggered);
    }

    private void OnBatchTriggered(int batch)
    {
        foreach (var uiTransitionBinding in bindings)
        {
            if (uiTransitionBinding.batch != batch) continue;

            registerElementEvent.Raise(1);

            if (uiTransitionBinding.action == UITransitionAction.Enter)
            {
                MoveToEnterPosition();
            }
            else
            {
                MoveToExitPosition();
            }

            return;
        }
    }
    
    // ENTER (SHOW UI)
    private void MoveToEnterPosition()
    {
        rectTransform.DOKill();

        rectTransform.DOAnchorPos(enterPosition, tween.enterDuration)
            .SetEase(tween.enterEase)
            .SetDelay(tween.enterDelay)
            .SetUpdate(true)
            .OnComplete(() => elementCompleteEvent.Raise());
    }
    
    // EXIT (HIDE / SIDE SHIFT / OFFSCREEN)
    private void MoveToExitPosition()
    {
        rectTransform.DOKill();

        rectTransform.DOAnchorPos(exitPosition, tween.exitDuration)
            .SetEase(tween.exitEase)
            .SetDelay(tween.exitDelay)
            .SetUpdate(true)
            .OnComplete(() => elementCompleteEvent.Raise());
    }
}

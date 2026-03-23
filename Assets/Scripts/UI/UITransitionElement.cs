using System;
using DG.Tweening;
using UnityEngine;

public class UITransitionElement : MonoBehaviour
{
    [Header("UI Transition Value")] 
    [SerializeField] private UITransitionElementValue UITransitionElementValue;
    
    [Header("RectTransform")]
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Move Settings")]
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float duration;

    [Header("Delays")]
    [SerializeField] private float moveOutDelay;
    [SerializeField] private float moveInDelay;
    
    [Header("Tween Settings")]
    [SerializeField] private Ease moveOutEase;
    [SerializeField] private Ease moveInEase;
    
    private Vector2 _originalPos;
    
    public event Action OnMoveOutComplete;
    public event Action OnMoveInComplete;

    private void Awake()
    {
        _originalPos = rectTransform.anchoredPosition;
        UITransitionElementValue.Bind(this);
    }

    public void MoveOut()
    {
        rectTransform.DOKill();
        
        rectTransform.DOAnchorPos(targetPosition, duration)
            .SetEase(moveOutEase)
            .SetDelay(moveOutDelay)
            .SetUpdate(true)
            .OnComplete(OnMoveOutCompleteEvent);
    }

    public void MoveIn()
    {
        rectTransform.DOKill();
        
        rectTransform.DOAnchorPos(_originalPos, duration)
            .SetEase(moveInEase)
            .SetDelay(moveInDelay)
            .SetUpdate(true)
            .OnComplete(OnMoveInCompleteEvent);
    }
    
    private void OnMoveOutCompleteEvent()
    {
        OnMoveOutComplete?.Invoke();
        OnMoveOutComplete = null;
    }

    private void OnMoveInCompleteEvent()
    {
        OnMoveInComplete?.Invoke();
        OnMoveInComplete = null;
    }
}

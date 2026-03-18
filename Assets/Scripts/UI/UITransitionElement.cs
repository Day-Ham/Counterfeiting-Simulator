using DG.Tweening;
using UnityEngine;

public class UITransitionElement : MonoBehaviour
{
    [SerializeField] private UITransitionManagerValue managerValue;

    [Header("RectTransform")]
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Move Settings")]
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float duration;
    [SerializeField] private float delay;
    
    [Header("Tween Settings")]
    [SerializeField] private Ease moveOutEase;
    [SerializeField] private Ease moveInEase;
    
    private Vector2 originalPos;

    private void Awake()
    {
        originalPos = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        managerValue?.Register(this);
    }

    private void OnDisable()
    {
        managerValue?.Unregister(this);
    }

    public void MoveOut()
    {
        rectTransform.DOAnchorPos(targetPosition, duration)
            .SetEase(moveOutEase)
            .SetDelay(delay)
            .SetUpdate(true);
    }

    public void MoveIn()
    {
        rectTransform.DOAnchorPos(originalPos, duration)
            .SetEase(moveInEase)
            .SetDelay(delay)
            .SetUpdate(true);
    }
}

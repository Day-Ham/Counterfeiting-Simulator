using DG.Tweening;
using UnityEngine;

public class UITransitionElement : MonoBehaviour
{
    [SerializeField] private UITransitionManagerValue managerValue;

    [Header("Move Settings")]
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease moveOutEase = Ease.InBack;
    [SerializeField] private Ease moveInEase = Ease.OutBack;

    private RectTransform rectTransform;
    private Vector2 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
            .SetUpdate(true);
    }

    public void MoveIn()
    {
        rectTransform.DOAnchorPos(originalPos, duration)
            .SetEase(moveInEase)
            .SetUpdate(true);
    }
}

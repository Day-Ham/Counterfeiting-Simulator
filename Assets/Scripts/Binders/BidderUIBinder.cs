using DG.Tweening;
using UnityEngine;

public class BidderUIBinder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform container;

    [Header("Positions")]
    [SerializeField] private Vector2 restPosition;
    [SerializeField] private Vector2 raisedPosition;

    [Header("Timing")]
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float holdTime = 0.2f;

    private Tween _currentTween;

    private void Awake()
    {
        // Safer than Start (runs earlier)
        if (container != null)
        {
            container.anchoredPosition = restPosition;
        }
    }

    public void Raise()
    {
        if (!container) return;

        // Kill safely (prevents stacking bugs)
        _currentTween?.Kill();

        _currentTween = DOTween.Sequence()
            .Append(container.DOAnchorPos(raisedPosition, duration).SetEase(Ease.OutBack))
            .AppendInterval(holdTime)
            .Append(container.DOAnchorPos(restPosition, duration).SetEase(Ease.InQuad))
            .SetLink(gameObject); // auto-kill if object is destroyed
    }

    private void OnDisable()
    {
        // Prevent ghost tween when object gets disabled
        _currentTween?.Kill();
    }
}

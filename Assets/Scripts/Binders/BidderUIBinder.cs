using DG.Tweening;
using UnityEngine;

public class BidderUIBinder : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private BidEvent onBidRaisedEvent;
    
    [Header("References")]
    [SerializeField] private RectTransform container;

    [Header("Positions")]
    [SerializeField] private Vector2 restPosition;
    [SerializeField] private Vector2 raisedPosition;

    [Header("Timing")]
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float holdTime = 0.2f;

    private Tween _currentTween;
    
    private NPCBidderRuntime _boundBidder;
    
    public void Bind(NPCBidderRuntime bidder)
    {
        _boundBidder = bidder;
    }

    private void OnEnable()
    {
        onBidRaisedEvent.Register(OnBidRaised);
    }

    private void OnDisable()
    {
        onBidRaisedEvent.Unregister(OnBidRaised);
        
        // Prevent ghost tween when object gets disabled
        _currentTween?.Kill();
    }
    
    private void Awake()
    {
        // Safer than Start (runs earlier)
        if (container != null)
        {
            container.anchoredPosition = restPosition;
        }
    }
    
    private void OnBidRaised(NPCBidderRuntime bidder)
    {
        // Only react if THIS UI belongs to the bidder
        if (bidder != _boundBidder) return;

        Raise();
    }

    private void Raise()
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
}

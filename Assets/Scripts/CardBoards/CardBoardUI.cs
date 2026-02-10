using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardBoardUI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxYOffset = 30f;
    [SerializeField] private float moveDuration = 0.8f;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceAmount = 5f;
    [SerializeField] private float bounceDuration = 0.2f;
    [SerializeField] private int bounceCount = 2;

    [Header("Pause Settings")]
    [SerializeField] private float pauseAfterBounce = 0.3f;
    [SerializeField] private float pauseAfterDown = 0.2f;

    private RectTransform rectTransform;
    private float startYPosition;
    private bool isActive = false;

    [Header("Visual FeedBack")]
    [SerializeField] private Image cardBoardImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private float colorTransitionDuration = 0.3f;

    private Sequence currentMoveSequence;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startYPosition = rectTransform.anchoredPosition.y;
    }

    void Start()
    {
        if (cardBoardImage != null)
        {
            cardBoardImage.color = normalColor;
        }
    }

    void MoveToDirection()
    {
        if (currentMoveSequence != null)
        {
            currentMoveSequence.Kill(true);
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startYPosition);

        Vector2 startPos = new Vector2(rectTransform.anchoredPosition.x, startYPosition);
        Vector2 topPos = new Vector2(rectTransform.anchoredPosition.x, startYPosition + maxYOffset);

        Vector2 topUpPos = new Vector2(rectTransform.anchoredPosition.x, startYPosition + maxYOffset + bounceAmount);
        Vector2 topDownPos = new Vector2(rectTransform.anchoredPosition.x, startYPosition + maxYOffset - bounceAmount);

        currentMoveSequence = DOTween.Sequence();

        currentMoveSequence.Append(
            rectTransform.DOAnchorPos(topPos, moveDuration)
                .SetEase(Ease.OutQuad)
        );

        for (int i = 0; i < bounceCount; i++)
        {
            currentMoveSequence.Append(
                rectTransform.DOAnchorPos(topUpPos, bounceDuration)
                    .SetEase(Ease.InOutSine)
            );

            currentMoveSequence.Append(
                rectTransform.DOAnchorPos(topDownPos, bounceDuration)
                    .SetEase(Ease.InOutSine)
            );
        }

        currentMoveSequence.Append(
            rectTransform.DOAnchorPos(topPos, bounceDuration)
                .SetEase(Ease.InOutSine)
        );

        currentMoveSequence.AppendInterval(pauseAfterBounce);

        currentMoveSequence.Append(
            rectTransform.DOAnchorPos(startPos, moveDuration)
                .SetEase(Ease.InOutQuad)
        );

        currentMoveSequence.AppendInterval(pauseAfterDown);

        currentMoveSequence.SetLoops(-1, LoopType.Restart);
    }

    public void SetActive(bool active)
    {
        isActive = active;

        if (cardBoardImage != null)
        {
            cardBoardImage.DOColor(active ? activeColor : normalColor, colorTransitionDuration);
        }

        if (active)
        {
            MoveToDirection();
        }
        else
        {
            if (currentMoveSequence != null)
            {
                currentMoveSequence.Kill(true);
                currentMoveSequence = null;
            }
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        rectTransform.DOKill();
        Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, startYPosition);
        rectTransform.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutBack);
    }

    private void OnDestroy()
    {
        if (currentMoveSequence != null)
        {
            currentMoveSequence.Kill();
        }
        rectTransform.DOKill();
    }
}
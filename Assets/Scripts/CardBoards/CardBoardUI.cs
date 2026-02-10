using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardBoardUI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float changeDirectionInterval = 3f;
    [SerializeField] private float minYOffset = -50f;
    [SerializeField] private float maxYOffset = 50f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private Ease movementEaseType = Ease.Linear;

    private RectTransform rectTransform;
    private float currentDirection = 1f;
    private float timeSinceLastChange = 0f;
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
        currentDirection = 1f;

        if (cardBoardImage != null)
        {
            cardBoardImage.color = normalColor;
        }
    }

    void Update()
    {
        if (isActive)
        {
            HandleMovement();
        }
    }

    public void HandleMovement()
    {
        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange >= changeDirectionInterval)
        {
            currentDirection = 1f;
            timeSinceLastChange = 0f;
            MoveToDirection(currentDirection);
        }
    }

    void MoveToDirection(float direction)
    {
        if (currentMoveSequence != null && currentMoveSequence.IsActive())
        {
            currentMoveSequence.Kill();
        }

        float upY = startYPosition + maxYOffset;
        float downY = startYPosition + minYOffset;
        Vector2 upPos = new Vector2(rectTransform.anchoredPosition.x, upY);
        Vector2 downPos = new Vector2(rectTransform.anchoredPosition.x, downY);

        currentMoveSequence = DOTween.Sequence();

        if (direction > 0)
        {
            currentMoveSequence.Append(rectTransform.DOAnchorPos(upPos, moveDuration).SetEase(movementEaseType));
            currentMoveSequence.Append(rectTransform.DOAnchorPos(downPos, moveDuration).SetEase(movementEaseType));
        }
        else
        {
            currentMoveSequence.Append(rectTransform.DOAnchorPos(downPos, moveDuration).SetEase(movementEaseType));
            currentMoveSequence.Append(rectTransform.DOAnchorPos(upPos, moveDuration).SetEase(movementEaseType));
        }

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
            timeSinceLastChange = 0f;
            currentDirection = 1f;
            MoveToDirection(currentDirection);
        }
        else
        {
            if (currentMoveSequence != null && currentMoveSequence.IsActive())
            {
                currentMoveSequence.Kill();
            }
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, startYPosition);
        rectTransform.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutBack);
    }
}
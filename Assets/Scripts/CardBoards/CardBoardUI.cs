using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardBoardUI : MonoBehaviour
{
    [Header("CardBoardUI Settings")]
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float changeDirectionInterval = 1f;
    [SerializeField] private float minYOffset = -50f;
    [SerializeField] private float maxYOffset = 50f;
    [SerializeField] private Ease movementEaseType = Ease.InOutSine;

    private RectTransform rectTransform;
    private float currentDirection = 1f;
    private float timeSinceLastChange = 0f;
    private float startYPosition;

    [SerializeField] private bool isActive = false;

    [Header("Visual FeedBack")]
    [SerializeField] private Image cardBoardImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private float colorTransitionDuration = 0.3f;

    Sequence moveSequence = DOTween.Sequence();
    private Sequence currentMoveSequence;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startYPosition = rectTransform.anchoredPosition.y;
    }
    void Start()
    {
        currentDirection = Random.value > 0.5f ? 1f : 1f;
    }

    void Update()
    {
        if (isActive)
        {
            HandleMovement();
        }
    }

    public void HandleMovement ()
    {
        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange >= changeDirectionInterval)
        {
            currentDirection = Random.value > 0.5f ? 1f : -1f;
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

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(rectTransform.DOAnchorPos(upPos, moveDuration * 0.5f).SetEase(movementEaseType));
        moveSequence.Append(rectTransform.DOAnchorPos(downPos, moveDuration * 0.5f).SetEase(movementEaseType));

        currentMoveSequence = moveSequence;
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
            currentDirection = Random.value > 0.5f ? 1f : -1f;
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
        Vector2 currentPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2 (currentPos.x, startYPosition);
    }
}

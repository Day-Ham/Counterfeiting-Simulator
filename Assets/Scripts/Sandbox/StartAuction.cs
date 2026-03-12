using DG.Tweening;
using UnityEngine;

public class StartAuction : MonoBehaviour
{
    [Header("References")]
    public GameObjectValue DrawingBoard;
    public GameObjectValue TargetLocation;
    public GameObjectValue DrawingBoardFrame;

    [Header("Tween Settings")]
    public float Duration = 1f;
    public Ease TweenEase = Ease.OutQuad;

    private bool hasMoved = false;
    
    // Cached RectTransforms
    private RectTransform boardRect;
    private RectTransform targetRect;
    private RectTransform frameRect;
    
    private void Awake()
    {
        boardRect = DrawingBoard.Value.GetComponent<RectTransform>();
        targetRect = TargetLocation.Value.GetComponent<RectTransform>();
        frameRect = DrawingBoardFrame.Value.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || hasMoved) return;
        
        MoveDrawingBoard();
    }

    private void MoveDrawingBoard()
    {
        if (boardRect && frameRect && targetRect)
        {
            boardRect.DOAnchorPos(targetRect.anchoredPosition, Duration).SetEase(TweenEase);
            
            frameRect.DOAnchorPos(targetRect.anchoredPosition, Duration).SetEase(TweenEase);
        }

        hasMoved = true;
    }
}

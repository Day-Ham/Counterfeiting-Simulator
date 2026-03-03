using UnityEngine;

public class DrawingBoardController : MonoBehaviour
{
    [SerializeField] private Canvas targetImageCanvas;
    [SerializeField] private Canvas drawingCanvas;
    [SerializeField] private VoidEvent compareStartedEvent;
    [SerializeField] private DrawingBoardZoom drawingBoardZoom;
    
    public RectTransform drawingBoard;
    public float snapSmoothness = 10f;

    private Vector2 originalSize;
    private Vector2 originalPosition;

    private bool isSnapRequested;
    private int initialDrawingCanvasSortingOrder = 1;
    private int initialTragetImageCanvasSortingOrder = 2;

    public Vector2 OriginalSize => originalSize;
    public Vector2 OriginalPosition => originalPosition;
    
    private void OnEnable()
    {
        compareStartedEvent.Register(SnapToOriginalWithSortingReset);
    }

    private void OnDisable()
    {
        compareStartedEvent.Unregister(SnapToOriginalWithSortingReset);
    }

    private void Awake()
    {
        InitializedCanvas();
    }

    private void InitializedCanvas()
    {
        targetImageCanvas.overrideSorting = true;
        targetImageCanvas.sortingOrder = initialTragetImageCanvasSortingOrder;
        
        drawingCanvas.overrideSorting = true;
        drawingCanvas.sortingOrder = initialDrawingCanvasSortingOrder;
        
        originalSize = drawingBoard.sizeDelta;
        originalPosition = drawingBoard.anchoredPosition;
    }

    private void Update()
    {
        HandleInput();
        UpdateSnap();
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SnapToOriginalPositionOnly();
        }
    }
    
    private void UpdateSnap()
    {
        if (!isSnapRequested) return;
        
        drawingBoard.sizeDelta = Vector2.Lerp(drawingBoard.sizeDelta, originalSize, Time.deltaTime * snapSmoothness);
        drawingBoard.anchoredPosition = Vector2.Lerp(drawingBoard.anchoredPosition, originalPosition, Time.deltaTime * snapSmoothness);
        
        if (!IsSnapComplete()) return;
        
        drawingBoard.sizeDelta = originalSize;
        drawingBoard.anchoredPosition = originalPosition;
        isSnapRequested = false;
    }
    
    private bool IsSnapComplete()
    {
        return Vector2.Distance(drawingBoard.sizeDelta, originalSize) < 0.1f && Vector2.Distance(drawingBoard.anchoredPosition, originalPosition) < 0.1f;
    }

    /// <summary>
    /// Snap to original and reset sorting (called by Compare Event)
    /// </summary>
    private void SnapToOriginalWithSortingReset()
    {
        targetImageCanvas.overrideSorting = false;
        drawingCanvas.overrideSorting = false;
        ResetToOriginalSize();
    }

    /// <summary>
    /// Snap to original position/size only (called by Q key)
    /// </summary>
    private void SnapToOriginalPositionOnly()
    {
        ResetToOriginalSize();
    }

    private void ResetToOriginalSize()
    {
        isSnapRequested = true;
        drawingBoardZoom.SetTargetSize(OriginalSize);
    }
}

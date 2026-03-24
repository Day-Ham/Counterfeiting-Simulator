using UnityEngine;
using UnityEngine.UI;

public class DrawingBoardController : MonoBehaviour
{
    [SerializeField] private DrawingBoardControllerValue _drawingBoardControllerValue;
    
    [SerializeField] private Canvas targetImageCanvas;
    [SerializeField] private Canvas drawingCanvas;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private VoidEvent compareStartedEvent;
    [SerializeField] private VoidEvent resetDrawingBoardPositionEvent;
    [SerializeField] private DrawingBoardZoom drawingBoardZoom;
    
    public RectTransform drawingBoard;
    public float snapSmoothness;

    private Vector2 originalSize;
    private Vector2 originalPosition;

    private bool isSnapRequested;
    private int initialDrawingCanvasSortingOrder = 1;
    private int initialTragetImageCanvasSortingOrder = 2;

    public Vector2 OriginalSize => originalSize;
    public Vector2 OriginalPosition => originalPosition;
    public bool IsCanInteract { get; private set; } = true;
    private bool IsDisableCtrlInput { get; set; } = false;

    private void OnEnable()
    {
        compareStartedEvent.Register(SnapToOriginalWithSortingReset);
        resetDrawingBoardPositionEvent.Register(SnapToOriginalPositionOnly);
        
        GameState.OnGameFinished += HandleGameFinished;
        GameState.OnGameStarted += HandleGameStarted;
    }

    private void OnDisable()
    {
        compareStartedEvent.Unregister(SnapToOriginalWithSortingReset);
        resetDrawingBoardPositionEvent.Unregister(SnapToOriginalPositionOnly);
        
        GameState.OnGameFinished -= HandleGameFinished;
        GameState.OnGameStarted -= HandleGameStarted;
    }

    private void Awake()
    {
        InitializedCanvas();

        _drawingBoardControllerValue.Value = this;
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
        UpdateSnap();
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
        
        IsCanInteract = true;
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
        DisableBoardInteraction();
        drawingBoardZoom.SetTargetSize(OriginalSize);
    }
    
    private void HandleGameStarted()
    {
        IsDisableCtrlInput = false;
        IsCanInteract = true;
    }
    
    private void HandleGameFinished()
    {
        DisableCtrlInput();
        DisableBoardInteraction();
    }

    private void DisableCtrlInput()
    {
        IsDisableCtrlInput = true;
    }
   
    public bool IsCanUseCtrl()
    {
        return !IsDisableCtrlInput && IsCanInteract;
    }
    
    private void DisableBoardInteraction()
    {
        IsCanInteract = false; // stops drag or zoom
    }
}

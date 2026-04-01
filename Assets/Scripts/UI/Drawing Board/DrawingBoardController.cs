using UnityEngine;
using UnityEngine.UI;

public class DrawingBoardController : MonoBehaviour
{
    [SerializeField] private Canvas targetImageCanvas;
    [SerializeField] private Canvas drawingCanvas;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private VoidEvent compareStartedEvent;
    [SerializeField] private VoidEvent resetDrawingBoardPositionEvent;
    [SerializeField] private DrawingBoardZoom drawingBoardZoom;
    
    public RectTransform drawingBoard;
    public float snapSmoothness;

    private Vector2 _originalSize;
    private Vector2 _originalPosition;

    private bool _isSnapRequested;

    private const int InitialDrawingCanvasSortingOrder = 1;
    private const int InitialTargetImageSortingOrder = 2;

    public Vector2 OriginalSize => _originalSize;
    public Vector2 OriginalPosition => _originalPosition;
    public bool IsCanInteract { get; private set; } = true;
    private bool IsDisableCtrlInput { get; set; } = false;

    private void OnEnable()
    {
        compareStartedEvent.Register(SnapToOriginalWithSortingReset);
        resetDrawingBoardPositionEvent.Register(SnapToOriginalPositionOnly);
        
        GameState.OnGameFinished += HandleGameFinished;
        GameState.OnGameStarted += HandleGameStarted;
        
        GameState.OnGamePaused += HandleGamePaused;
        GameState.OnGameResumed += HandleGameResumed; 
    }

    private void OnDisable()
    {
        compareStartedEvent.Unregister(SnapToOriginalWithSortingReset);
        resetDrawingBoardPositionEvent.Unregister(SnapToOriginalPositionOnly);
        
        GameState.OnGameFinished -= HandleGameFinished;
        GameState.OnGameStarted -= HandleGameStarted;
        
        GameState.OnGamePaused -= HandleGamePaused;
        GameState.OnGameResumed -= HandleGameResumed;
    }

    private void Awake()
    {
        InitializedCanvas();
    }

    private void InitializedCanvas()
    {
        targetImageCanvas.overrideSorting = true;
        targetImageCanvas.sortingOrder = InitialTargetImageSortingOrder;
        
        drawingCanvas.overrideSorting = true;
        drawingCanvas.sortingOrder = InitialDrawingCanvasSortingOrder;
        
        _originalSize = drawingBoard.sizeDelta;
        _originalPosition = drawingBoard.anchoredPosition;
    }

    private void Update()
    {
        UpdateSnap();
    }
    
    private void UpdateSnap()
    {
        if (!_isSnapRequested) return;
        
        drawingBoard.sizeDelta = Vector2.Lerp(drawingBoard.sizeDelta, _originalSize, Time.deltaTime * snapSmoothness);
        drawingBoard.anchoredPosition = Vector2.Lerp(drawingBoard.anchoredPosition, _originalPosition, Time.deltaTime * snapSmoothness);
        
        if (!IsSnapComplete()) return;
        
        drawingBoard.sizeDelta = _originalSize;
        drawingBoard.anchoredPosition = _originalPosition;
        _isSnapRequested = false;
        
        IsCanInteract = true;
    }
    
    private bool IsSnapComplete()
    {
        return Vector2.Distance(drawingBoard.sizeDelta, _originalSize) < 0.1f && Vector2.Distance(drawingBoard.anchoredPosition, _originalPosition) < 0.1f;
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
        _isSnapRequested = true;
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
    
    private void HandleGamePaused()
    {
        DisableBoardInteraction();
    }

    private void DisableCtrlInput()
    {
        IsDisableCtrlInput = true;
    }
    
    private void HandleGameResumed()
    {
        // Only re-enable if not finished
        if (!GameState.IsGameFinished)
        {
            IsCanInteract = true;
        }
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

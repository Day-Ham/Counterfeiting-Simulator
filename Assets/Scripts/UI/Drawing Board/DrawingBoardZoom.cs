using System;
using UnityEngine;

public class DrawingBoardZoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DrawingBoardController boardController;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f;
    public float zoomSmoothness = 10f;
    public Vector2 minSize = new Vector2(200, 200);
    public Vector2 maxSize = new Vector2(2000, 2000);
    
    private Vector2 _targetSize;
    
    //Properties//
    private RectTransform DrawBoard => boardController.drawingBoard;
    private Vector2 OriginalSize => boardController.OriginalSize;
    public float ZoomRatio => DrawBoard.sizeDelta.x / OriginalSize.x;
    
    private void Start()
    {
        _targetSize = DrawBoard.sizeDelta;
    }

    private void Update()
    {
        if (!InputUtility.IsCtrlHeld) return;

        HandleScrollZoom();
        SmoothZoom();
    }

    private void HandleScrollZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0f)
        {
            ApplyZoom(scroll);
        }
    }

    private void SmoothZoom()
    {
        if (Vector2.Distance(DrawBoard.sizeDelta, _targetSize) > 0.01f)
        {
            DrawBoard.sizeDelta = Vector2.Lerp(DrawBoard.sizeDelta, _targetSize, Time.deltaTime * zoomSmoothness);
        }
    }

    private void ApplyZoom(float scrollAmount)
    {
        float factor = 1f + scrollAmount * zoomSpeed;
        _targetSize *= factor;

        _targetSize.x = Mathf.Clamp(_targetSize.x, minSize.x, maxSize.x);
        _targetSize.y = Mathf.Clamp(_targetSize.y, minSize.y, maxSize.y);
    }
}

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
    private RectTransform DrawBoardRectTransform => boardController.drawingBoard;
    private Vector2 DrawBoardOriginalSize => boardController.OriginalSize;
    public float ZoomRatio => DrawBoardRectTransform.sizeDelta.x / DrawBoardOriginalSize.x;
    
    private void Start()
    {
        _targetSize = DrawBoardRectTransform.sizeDelta;
    }

    private void Update()
    {
        if (!InputUtility.IsCtrlHeld)
        {
            return;
        };

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            ApplyZoom(scroll);
        }
        
        if (Vector2.Distance(DrawBoardRectTransform.sizeDelta, _targetSize) > 0.01f)
        {
            DrawBoardRectTransform.sizeDelta = Vector2.Lerp(
                DrawBoardRectTransform.sizeDelta,
                _targetSize,
                Time.deltaTime * zoomSmoothness
            );
        }
    }

    private void ApplyZoom(float scrollAmount)
    {
        float factor = 1 + scrollAmount * zoomSpeed;

        _targetSize *= factor;

        _targetSize.x = Mathf.Clamp(_targetSize.x, minSize.x, maxSize.x);
        _targetSize.y = Mathf.Clamp(_targetSize.y, minSize.y, maxSize.y);
    }
}

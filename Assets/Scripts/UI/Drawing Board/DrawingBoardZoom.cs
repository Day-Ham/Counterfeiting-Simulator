using System;
using UnityEngine;

public class DrawingBoardZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public RectTransform drawingBoard;
    public float zoomSpeed = 0.1f;
    
    [Tooltip("Higher = faster, lower = smoother")]
    public float zoomSmoothness = 10f;
    public Vector2 minSize = new Vector2(200, 200);
    public Vector2 maxSize = new Vector2(2000, 2000);
    
    private Vector2 originalSize;
    private Vector2 targetSize;
    private Vector2 originalPosition;
    private float targetPosY;
    
    public float ZoomRatio => drawingBoard.sizeDelta.x / originalSize.x;

    private void Start()
    {
        if (drawingBoard == null)
        {
            Debug.LogError("Assign the DrawingBoard RectTransform!");
            enabled = false;
            return;
        }

        originalSize = drawingBoard.sizeDelta;
        targetSize = originalSize;
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

        // Smooth zoom
        drawingBoard.sizeDelta = Vector2.Lerp(
            drawingBoard.sizeDelta,
            targetSize,
            Time.deltaTime * zoomSmoothness
        );
    }

    private void ApplyZoom(float scrollAmount)
    {
        float factor = 1 + scrollAmount * zoomSpeed;

        targetSize *= factor;

        targetSize.x = Mathf.Clamp(targetSize.x, minSize.x, maxSize.x);
        targetSize.y = Mathf.Clamp(targetSize.y, minSize.y, maxSize.y);
    }
}

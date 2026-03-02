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

        originalPosition = drawingBoard.anchoredPosition;
        targetPosY = originalPosition.y;
    }
    
    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) return;
        
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            ApplyZoom(scroll);
        }
        
        drawingBoard.sizeDelta = Vector2.Lerp(drawingBoard.sizeDelta, targetSize, Time.deltaTime * zoomSmoothness);
        
        Vector2 pos = drawingBoard.anchoredPosition;
        pos.y = Mathf.Lerp(pos.y, targetPosY, Time.deltaTime * zoomSmoothness);
        drawingBoard.anchoredPosition = pos;
    }

    private void ApplyZoom(float scrollAmount)
    {
        float factor = 1 + scrollAmount * zoomSpeed;
        
        targetSize = targetSize * factor;
        
        targetSize.x = Mathf.Clamp(targetSize.x, minSize.x, maxSize.x);
        targetSize.y = Mathf.Clamp(targetSize.y, minSize.y, maxSize.y);
        
        float zoomPercent = (targetSize.y - originalSize.y) / (maxSize.y - originalSize.y);
        zoomPercent = Mathf.Clamp01(zoomPercent);
        
        targetPosY = Mathf.Lerp(originalPosition.y, 0f, zoomPercent);
    }
}

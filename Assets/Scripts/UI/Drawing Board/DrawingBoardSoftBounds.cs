using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingBoardSoftBounds : MonoBehaviour
{
    [SerializeField] private float outsideAllowance;
    [SerializeField] private RectTransform drawingBoardRectTransform;
    [SerializeField] private RectTransform canvasRect;
    
    private float minX, maxX, minY, maxY;
    
    private void Awake()
    {
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        float canvasHalfW = canvasRect.rect.width * 0.5f;
        float canvasHalfH = canvasRect.rect.height * 0.5f;

        float boardHalfW = drawingBoardRectTransform.rect.width * 0.5f;
        float boardHalfH = drawingBoardRectTransform.rect.height * 0.5f;

        minX = -canvasHalfW + boardHalfW - outsideAllowance;
        maxX =  canvasHalfW - boardHalfW + outsideAllowance;

        minY = -canvasHalfH + boardHalfH - outsideAllowance;
        maxY =  canvasHalfH - boardHalfH + outsideAllowance;
    }

    /// <summary>
    /// Call this after updating anchoredPosition to clamp within soft bounds.
    /// </summary>
    public void ClampPosition()
    {
        Vector2 pos = drawingBoardRectTransform.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        drawingBoardRectTransform.anchoredPosition = pos;
    }
}

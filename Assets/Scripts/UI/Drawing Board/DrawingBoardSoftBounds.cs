using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingBoardSoftBounds : MonoBehaviour
{
    [SerializeField] private float outsideAllowance;
    [SerializeField] private RectTransform drawingBoardRectTransform;
    [SerializeField] private RectTransform canvasRect;
    
    private float _minClampX, _maxClampX, _minClampY, _maxClampY;
    
    private void Awake()
    {
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        float canvasHalfWidth = canvasRect.rect.width * 0.5f;
        float canvasHalfHeight = canvasRect.rect.height * 0.5f;

        float boardHalfWidth = drawingBoardRectTransform.rect.width * 0.5f;
        float boardHalfHeight = drawingBoardRectTransform.rect.height * 0.5f;

        _minClampX = -canvasHalfWidth + boardHalfWidth - outsideAllowance;
        _maxClampX =  canvasHalfWidth - boardHalfWidth + outsideAllowance;

        _minClampY = -canvasHalfHeight + boardHalfHeight - outsideAllowance;
        _maxClampY =  canvasHalfHeight - boardHalfHeight + outsideAllowance;
    }

    /// <summary>
    /// Call this after updating anchoredPosition to clamp within soft bounds.
    /// </summary>
    public void ClampPosition()
    {
        Vector2 vector2Position = drawingBoardRectTransform.anchoredPosition;
        vector2Position.x = Mathf.Clamp(vector2Position.x, _minClampX, _maxClampX);
        vector2Position.y = Mathf.Clamp(vector2Position.y, _minClampY, _maxClampY);
        drawingBoardRectTransform.anchoredPosition = vector2Position;
    }
}

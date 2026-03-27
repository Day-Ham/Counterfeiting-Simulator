using UnityEngine;

public static class ContextMenuPositionUtility
{
    public static void PositionAtCursor(RectTransform target, Canvas canvas, Vector2 screenPosition, Vector2 padding)
    {
        if (!target || !canvas) return;

        RectTransform canvasRect = canvas.transform as RectTransform;
        
        Vector2 pivot = new Vector2(
            screenPosition.x > Screen.width * 0.5f ? 1f : 0f,
            screenPosition.y > Screen.height * 0.5f ? 1f : 0f
        );

        target.pivot = pivot;

        // Convert screen -> local
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        // Apply padding so it doesn't overlap cursor
        localPoint += new Vector2(
            pivot.x == 0 ? padding.x : -padding.x,
            pivot.y == 0 ? padding.y : -padding.y
        );

        target.localPosition = localPoint;
    }
}

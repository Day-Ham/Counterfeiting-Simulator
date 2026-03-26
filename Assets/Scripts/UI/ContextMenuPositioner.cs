using UnityEngine;

public class ContextMenuPositioner : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private BoolEvent toggleContextMenu;

    [Header("UI")]
    [SerializeField] private RectTransform contextMenu;
    [SerializeField] private Canvas contextMenuCanvas;

    private void OnEnable()
    {
        toggleContextMenu.Register(OnToggleMousePosition);
    }

    private void OnDisable()
    {
        toggleContextMenu.Unregister(OnToggleMousePosition);
    }

    private void OnToggleMousePosition(bool value)
    {
        if (!value) return;

        SetPositionAtMouse();
    }

    private void SetPositionAtMouse()
    {
        if (!contextMenu || !contextMenuCanvas) return;

        RectTransform canvasRectTransform = contextMenuCanvas.transform as RectTransform;
        Vector2 mousePos = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            mousePos,
            contextMenuCanvas.worldCamera,
            out Vector2 localPoint
        );

        // Clamp inside screen
        Vector2 size = contextMenu.sizeDelta;

        if (!canvasRectTransform) return;
        
        float clampedX = Mathf.Clamp(localPoint.x,
            -canvasRectTransform.rect.width / 2 + size.x / 2,
            canvasRectTransform.rect.width / 2 - size.x / 2);

        float clampedY = Mathf.Clamp(localPoint.y,
            -canvasRectTransform.rect.height / 2 + size.y / 2,
            canvasRectTransform.rect.height / 2 - size.y / 2);

        contextMenu.localPosition = new Vector2(clampedX, clampedY);
    }
}

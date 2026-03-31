using UnityEngine;

public class ContextMenuPositioner : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private BoolEvent toggleContextMenu;

    [Header("UI")]
    [SerializeField] private RectTransform contextMenu;
    [SerializeField] private Canvas contextMenuCanvas;
    
    [Header("Settings")]
    [SerializeField] private Vector2 cursorPadding = new Vector2(4f, -4f);

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

        ContextMenuPositionUtility.PositionAtCursor(
            contextMenu,
            contextMenuCanvas,
            Input.mousePosition,
            cursorPadding
        );
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class CloseContextMenu : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private BoolEvent toggleContextMenu;

    [Header("UI")]
    [SerializeField] private RectTransform contextMenu;
    [SerializeField] private Canvas canvas;

    private Camera _uiCamera;

    private void Awake()
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            _uiCamera = null;
            return;
        }

        _uiCamera = canvas.worldCamera;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!EventSystem.current) return;

        if (IsClickInsideMenu()) return;

        HideContextMenu();
    }

    private bool IsClickInsideMenu()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(
            contextMenu,
            Input.mousePosition,
            _uiCamera
        );
    }

    private void HideContextMenu()
    {
        toggleContextMenu.Raise(false);
    }

}

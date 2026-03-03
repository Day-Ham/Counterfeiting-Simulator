using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingBoardDrag : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DrawingBoardController boardController;

    [Header("Drag Settings")]
    public float dragSpeed = 1f;

    private bool isDragging;
    private Vector2 lastMousePosition;
    
    //Properties//
    private RectTransform DrawBoardRectTransform => boardController.drawingBoard;

    private void Update()
    {
        if (!IsCanInteract()) return;
        if (!InputUtility.IsCtrlHeld) return;

        HandleMouseDown();
        HandleMouseUp();
        HandleDrag();
    }

    private void HandleMouseDown()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        isDragging = true;
        lastMousePosition = Input.mousePosition;
    }

    private void HandleMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void HandleDrag()
    {
        if (!isDragging) return;

        Vector2 currentMousePos = Input.mousePosition;
        Vector2 delta = currentMousePos - lastMousePosition;

        DrawBoardRectTransform.anchoredPosition += delta * dragSpeed;
        lastMousePosition = currentMousePos;
    }
    
    private bool IsCanInteract()
    {
        return boardController && boardController.IsCanInteract;
    }
}

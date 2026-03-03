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
    private RectTransform DrawBoardPosition => boardController.drawingBoard;

    private void Update()
    {
        if (!InputUtility.IsCtrlHeld)
        {
            return;
        };

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (!isDragging) return;
        
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 delta = currentMousePos - lastMousePosition;

        DrawBoardPosition.anchoredPosition += delta * dragSpeed;

        lastMousePosition = currentMousePos;
    }
}

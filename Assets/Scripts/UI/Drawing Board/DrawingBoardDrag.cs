using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingBoardDrag : MonoBehaviour
{
    public RectTransform drawingBoard;
    public float dragSpeed = 1f;

    private bool isDragging;
    private Vector2 lastMousePosition;

    private void Update()
    {
        bool ctrlHeld = InputUtility.IsCtrlHeld;

        if (ctrlHeld && Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 currentMousePos = Input.mousePosition;
            Vector2 delta = currentMousePos - lastMousePosition;

            drawingBoard.anchoredPosition += delta * dragSpeed;

            lastMousePosition = currentMousePos;
        }
    }
}

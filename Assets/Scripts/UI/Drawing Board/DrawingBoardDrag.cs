using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingBoardDrag : MonoBehaviour
{
    public RectTransform drawingBoard;
    public float dragSpeed = 1f;

    private bool isDragging;
    private Vector2 lastMousePosition;
    private Vector2 startingPosition;

    private void Start()
    {
        startingPosition = drawingBoard.anchoredPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            drawingBoard.anchoredPosition = startingPosition;
        }
        
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

        drawingBoard.anchoredPosition += delta * dragSpeed;

        lastMousePosition = currentMousePos;
    }
}

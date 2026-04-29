using UnityEngine;

public class DrawingBoardRotate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DrawingBoardController boardController;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f;
    public float rotationSmoothness = 10f;

    private float _targetRotation;

    private RectTransform DrawBoard => boardController.drawingBoard;

    private void Start()
    {
        _targetRotation = DrawBoard.localEulerAngles.z;
    }

    private void Update()
    {
        if (!boardController.IsCanUseCtrl()) return;
        if (!InputUtility.IsShiftHeld) return;

        HandleScrollRotation();
        SmoothRotate();
    }

    private void HandleScrollRotation()
    {
        float scroll = InputUtility.MouseWheelDelta;
        if (Mathf.Abs(scroll) > 0f)
            _targetRotation -= scroll * rotationSpeed;
    }

    private void SmoothRotate()
    {
        float current = DrawBoard.localEulerAngles.z;
        float angle = Mathf.LerpAngle(current, _targetRotation, Time.deltaTime * rotationSmoothness);
        DrawBoard.localEulerAngles = new Vector3(0f, 0f, angle);
    }

    public void SetTargetRotation(float rotation)
    {
        _targetRotation = rotation;
    }
}

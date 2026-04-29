using UnityEngine;

public static class InputUtility
{
    public static bool IsCtrlHeld => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    public static bool IsShiftHeld => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    public static float MouseWheelDelta => Input.mouseScrollDelta.y;
}

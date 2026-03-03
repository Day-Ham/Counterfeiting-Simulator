using UnityEngine;

public static class InputUtility
{
    public static bool IsCtrlHeld => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    public static float MouseWheelDelta => Input.mouseScrollDelta.y;
}

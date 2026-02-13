using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

public class CanvasDrawControllerAssignment : MonoBehaviour
{
    public CanvasDrawController CanvasDrawControllerTarget;
    public CanvasDrawControllerValue CanvasDrawControllerSO;

    private void Awake()
    {
        CanvasDrawControllerSO.Value = CanvasDrawControllerTarget;
    }
}

using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInputHandler", menuName = "Settings/Input Handler")]
public class InputHandler : ScriptableObject
{
    private CanvasDrawController _canvasDraw;
    private System.Action _finishGameCallback;

    public void Initialize(CanvasDrawController canvasDraw, System.Action finishGameCallback)
    {
        _canvasDraw = canvasDraw;
        _finishGameCallback = finishGameCallback;
    }
    
    private void OnDisable()
    {
        _canvasDraw = null;
        _finishGameCallback = null;
    }
    
    public void UpdateInput()
    {
        if (_canvasDraw == null) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _canvasDraw.SetBrushColorIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _canvasDraw.SetBrushColorIndex(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _canvasDraw.SetBrushColorIndex(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _canvasDraw.SetBrushColorIndex(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _canvasDraw.SetBrushColorIndex(4);
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _finishGameCallback?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _canvasDraw.UndoLastDraw();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _canvasDraw.ClearCurrentLayer();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Draw;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Erase;
        }
    }
}

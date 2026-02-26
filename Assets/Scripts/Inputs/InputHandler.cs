using System;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInputHandler", menuName = "Settings/Input Handler")]
public class InputHandler : ScriptableObject
{
    private CanvasDrawController _canvasDraw;
    private Action _finishGameCallback;
    
    private Dictionary<KeyCode, Action> _inputActions;

    public void Initialize(CanvasDrawController canvasDraw, Action finishGameCallback)
    {
        _canvasDraw = canvasDraw;
        _finishGameCallback = finishGameCallback;
        
        BuildInputDictionary();
    }
    
    private void BuildInputDictionary()
    {
        _inputActions = new Dictionary<KeyCode, Action>
        {
            // Brush Colors
            { KeyCode.Alpha1, () => _canvasDraw.SetBrushColorIndex(0) },
            { KeyCode.Alpha2, () => _canvasDraw.SetBrushColorIndex(1) },
            { KeyCode.Alpha3, () => _canvasDraw.SetBrushColorIndex(2) },
            { KeyCode.Alpha4, () => _canvasDraw.SetBrushColorIndex(3) },
            { KeyCode.Alpha5, () => _canvasDraw.SetBrushColorIndex(4) },

            // Actions
            { KeyCode.F, () => _finishGameCallback?.Invoke() },
            { KeyCode.Z, () => _canvasDraw.UndoLastDraw() },
            { KeyCode.C, () => _canvasDraw.ClearCurrentLayer() },
            { KeyCode.D, () => _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Draw },
            { KeyCode.E, () => _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Erase }
        };
    }

    public void UpdateInput()
    {
        if (!_canvasDraw || _inputActions == null)
        {
            return;
        };

        foreach (var input in _inputActions)
        {
            if (Input.GetKeyDown(input.Key))
            {
                input.Value?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        _canvasDraw = null;
        _finishGameCallback = null;
        _inputActions?.Clear();
    }
}

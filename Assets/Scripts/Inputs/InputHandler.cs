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
        _inputActions = new Dictionary<KeyCode, Action>();
        
        if (_canvasDraw == null || _canvasDraw.LevelConfigRuntime == null || _canvasDraw.LevelConfigRuntime.Value == null)
        {
            Debug.LogWarning("CanvasDrawController or LevelConfigRuntime not assigned!");
            return;
        }

        var colors = _canvasDraw.LevelConfigRuntime.Value.ColorsToBeUsed?.Value;
        int colorCount = colors?.Count ?? 0;

        // Dynamically bind Alpha keys to available colors
        for (int i = 0; i < colorCount; i++)
        {
            int index = i;
            if (i >= 9) break; // Only Alpha1–Alpha9 keys exist
            KeyCode key = KeyCode.Alpha1 + i;
            _inputActions[key] = () => _canvasDraw.SetBrushColorIndex(index);
        }
        
        _inputActions[KeyCode.F] = () => _finishGameCallback?.Invoke();
        _inputActions[KeyCode.Z] = () => _canvasDraw.UndoLastDraw();
        _inputActions[KeyCode.C] = () => _canvasDraw.ClearCurrentLayer();
        _inputActions[KeyCode.D] = () => _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Draw;
        _inputActions[KeyCode.E] = () => _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Erase;
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

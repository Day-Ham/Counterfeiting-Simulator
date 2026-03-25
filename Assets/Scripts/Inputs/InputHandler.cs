using System;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInputHandler", menuName = "Settings/Input Handler")]
public class InputHandler : ScriptableObject
{
    [SerializeField] private SelectBrushColorEvent SelectBrushColorEvent;
    [SerializeField] private VoidEvent ResetDrawingBoardPositionEvent;
    [SerializeField] private VoidEvent SpacePressedEvent;
    
    private CanvasDrawController _canvasDraw;
    private Action _finishGameCallback;
    private bool _isBlockInput;
    
    private Dictionary<KeyCode, Action> _inputActions;

    public void Initialize(CanvasDrawController canvasDraw, Action finishGameCallback)
    {
        UnsubscribeGameState();
        
        _canvasDraw = canvasDraw;
        _finishGameCallback = finishGameCallback;
        
        _isBlockInput = false;
        
        BuildInputDictionary();
        
        SubscribeGameState();
    }
    
    private void SubscribeGameState()
    {
        GameState.OnGameFinished += BlockInput;
        GameState.OnGameStarted += UnblockInput;

        GameState.OnGamePaused += BlockInput;
        GameState.OnGameResumed += UnblockInput;
    }

    private void UnsubscribeGameState()
    {
        GameState.OnGameFinished -= BlockInput;
        GameState.OnGameStarted -= UnblockInput;

        GameState.OnGamePaused -= BlockInput;
        GameState.OnGameResumed -= UnblockInput;
    }
    
    private void BuildInputDictionary()
    {
        _inputActions = new Dictionary<KeyCode, Action>();

        if (!IsValidCanvasDraw()) return;

        BindColorKeys();
        BindToolKeys();
    }
    
    private bool IsValidCanvasDraw()
    {
        if (_canvasDraw.RuntimeAsset.HasValue) return true;
        
        Debug.LogWarning("CanvasDrawController or LevelConfigRuntime not assigned!");
        return false;
    }
    
    private void BindColorKeys()
    {
        var colors = _canvasDraw.RuntimeAsset.GetActiveColors();
        
        int colorCount = colors?.Count ?? 0;

        for (int i = 0; i < Mathf.Min(colorCount, 9); i++)
        {
            int colorIndex = i;
            KeyCode key = KeyCode.Alpha1 + i;
            _inputActions[key] = () => SelectColor(colorIndex);
        }
        
        _inputActions[KeyCode.B] = () => SelectColor(0);
    }
    
    private void BindToolKeys()
    {
        _inputActions[KeyCode.Space] = () => SpacePressedEvent?.Raise();
        
        _inputActions[KeyCode.F] = () => _finishGameCallback?.Invoke();
        _inputActions[KeyCode.Z] = () => _canvasDraw.UndoLastDraw();
        _inputActions[KeyCode.C] = () => _canvasDraw.ClearCurrentLayer();
        _inputActions[KeyCode.D] = () => _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Draw;
        _inputActions[KeyCode.E] = () => SelectBrushColorEvent.RaiseErase();
        _inputActions[KeyCode.Q] = () => ResetDrawingBoardPositionEvent.Raise();
    }
    
    private void SelectColor(int index)
    {
        var colors = _canvasDraw.RuntimeAsset.GetActiveColors();
        
        if (colors != null && index < colors.Count)
        {
            SelectBrushColorEvent.Raise(index);
        };
    }

    public void UpdateInput()
    {
        if (!_canvasDraw || _inputActions == null || _isBlockInput)
        {
            return; // <-- ignore all input if blocked
        }
        
        foreach (var input in _inputActions)
        {
            if (Input.GetKeyDown(input.Key))
            {
                input.Value?.Invoke();
            }
        }
    }

    private void BlockInput()
    {
        _isBlockInput = true;
    }

    private void UnblockInput()
    {
        _isBlockInput = false;
    }

    private void OnDisable()
    {
        UnsubscribeGameState();
        
        _canvasDraw = null;
        _finishGameCallback = null;
        _inputActions?.Clear();
    }
}

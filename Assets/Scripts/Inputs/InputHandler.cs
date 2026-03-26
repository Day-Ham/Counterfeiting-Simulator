using System;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInputHandler", menuName = "Settings/Input Handler")]
public class InputHandler : ScriptableObject
{
    [Header("Events")]
    [SerializeField] private IntEvent colorSelectedEvent;
    [SerializeField] private VoidEvent eraserSelectEvent;
    [SerializeField] private OpenColorPickerEvent openColorPickerEvent;
    [SerializeField] private VoidEvent resetDrawingBoardPositionEvent;
    [SerializeField] private VoidEvent spacePressedEvent;
    [SerializeField] private VoidEvent undoDrawEvent;
    
    private CanvasDrawController _canvasDraw;
    private Action _finishGameCallback;
    
    private bool _isBlockInputGameState;
    private bool _isBlockInputColorPicker; 
    
    private Dictionary<KeyCode, Action> _colorKeyActions;
    private Dictionary<KeyCode, Action> _toolKeyActions;

    public void Initialize(CanvasDrawController canvasDraw, Action finishGameCallback)
    {
        UnsubscribeGameState();
        
        _canvasDraw = canvasDraw;
        _finishGameCallback = finishGameCallback;
        
        _isBlockInputGameState = false;
        
        BuildInputDictionary();
        
        openColorPickerEvent.RegisterToggleBool(OnColorPickerToggle);
        
        SubscribeGameState();
    }
    
    private void OnColorPickerToggle(bool isColorPickerUIOpen)
    {
        _isBlockInputColorPicker = isColorPickerUIOpen;
    }
    
    private void SubscribeGameState()
    {
        GameState.OnGameFinished += OnGameFinished;
        GameState.OnGameStarted += OnGameStarted;
        
        GameState.OnGamePaused += OnGamePaused;
        GameState.OnGameResumed += OnGameResumed;
    }

    private void UnsubscribeGameState()
    {
        GameState.OnGameFinished -= OnGameFinished;
        GameState.OnGameStarted -= OnGameStarted;
        
        GameState.OnGamePaused -= OnGamePaused;
        GameState.OnGameResumed -= OnGameResumed;
        
        openColorPickerEvent.UnregisterToggleBool(OnColorPickerToggle);
    }
    
    private void BuildInputDictionary()
    {
        _colorKeyActions = new Dictionary<KeyCode, Action>();
        _toolKeyActions = new Dictionary<KeyCode, Action>();

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
            _colorKeyActions[key] = () => SelectColor(colorIndex);
        }
        
        _colorKeyActions[KeyCode.B] = () => SelectColor(0);
    }
    
    private void BindToolKeys()
    {
        _toolKeyActions[KeyCode.Space] = () => spacePressedEvent?.Raise();
        
        _toolKeyActions[KeyCode.F] = () => _finishGameCallback?.Invoke();
        _toolKeyActions[KeyCode.Z] = () => undoDrawEvent?.Raise();
        _toolKeyActions[KeyCode.C] = () => _canvasDraw.ClearCurrentLayer();
        _toolKeyActions[KeyCode.D] = () => _canvasDraw.CurrentDrawMode = CanvasDrawController.DrawMode.Draw;
        _toolKeyActions[KeyCode.E] = () => eraserSelectEvent.Raise();
        _toolKeyActions[KeyCode.Q] = () => resetDrawingBoardPositionEvent.Raise();
    }
    
    private void SelectColor(int index)
    {
        var colors = _canvasDraw.RuntimeAsset.GetActiveColors();
        
        if (colors != null && index < colors.Count)
        {
            colorSelectedEvent.Raise(index);
        }
    }

    public void UpdateInput()
    {
        if (!_canvasDraw) return;

        // Color keys are blocked if either game state or color picker blocks them
        if (!_isBlockInputGameState && !_isBlockInputColorPicker && _colorKeyActions != null)
        {
            foreach (var inputAlphaKeyCode in _colorKeyActions)
            {
                if (Input.GetKeyDown(inputAlphaKeyCode.Key))
                {
                    inputAlphaKeyCode.Value?.Invoke();
                }
            }
        }

        // Tool keys are blocked only by game state
        if (!_isBlockInputGameState && _toolKeyActions != null)
        {
            foreach (var inputAlphaKeyCode in _toolKeyActions)
            {
                if (Input.GetKeyDown(inputAlphaKeyCode.Key))
                {
                    inputAlphaKeyCode.Value?.Invoke();
                }
            }
        }
    }
    
    // Game state handlers
    private void OnGameFinished()
    {
        _isBlockInputGameState = true;
    }
    
    private void OnGamePaused()
    {
        _isBlockInputGameState = true;
    }
    
    private void OnGameStarted()
    {
        _isBlockInputGameState = false;
    }
    
    private void OnGameResumed()
    { 
        _isBlockInputGameState = false;
    }
    
    private void OnDisable()
    {
        UnsubscribeGameState();
        
        _canvasDraw = null;
        _finishGameCallback = null;
        _toolKeyActions?.Clear();
    }
}

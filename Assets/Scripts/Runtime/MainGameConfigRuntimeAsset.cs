using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/New Level Config")]
public class MainGameConfigRuntimeAsset : ConfigRuntime
{
    private MainGameModeConfig _runtimeValue;

    public override event Action OnValueChanged;
    
    public override bool UseSnapping => true;

    public MainGameModeConfig Value
    {
        get => _runtimeValue;
        set
        {
            if (_runtimeValue == value) return;

            _runtimeValue = value;
            OnValueChanged?.Invoke();
        }
    }
    
    public override bool HasValue => _runtimeValue != null;

    public override List<Color> GetActiveColors()
    {
        if (_runtimeValue == null) return null;
        return _runtimeValue.GetActiveColors();
    }
    
    public override int UndoLimit
    {
        get
        {
            if (_runtimeValue == null || _runtimeValue.UndoLimit == null)
                return 0;

            return _runtimeValue.UndoLimit.Value;
        }
    }

    public override int StrokeLimit
    {
        get
        {
            if (_runtimeValue == null || _runtimeValue.StrokeLimit == null)
                return 0;
            return _runtimeValue.StrokeLimit.Value;
        }
    }
}

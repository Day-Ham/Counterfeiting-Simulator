using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/New Level Config")]
public class LevelConfigRuntimeAsset : ConfigRuntime
{
    private LevelConfig _runtimeValue;

    public override event Action OnValueChanged;

    public LevelConfig Value
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
}

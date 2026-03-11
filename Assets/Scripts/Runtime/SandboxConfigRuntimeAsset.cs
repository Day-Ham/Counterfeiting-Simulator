using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/New Sandbox Config")]
public class SandboxConfigRuntimeAsset : ConfigRuntime
{
    private SandboxModeConfig _runtimeValue;

    public override event Action OnValueChanged;

    public SandboxModeConfig Value
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

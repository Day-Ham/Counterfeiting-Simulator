using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/New Sandbox Config")]
public class SandboxConfigRuntimeAsset : ScriptableObject
{
    private SandboxModeConfig _runtimeValue;
    public event Action<SandboxModeConfig> OnValueChanged;
    public SandboxModeConfig Value
    {
        get => _runtimeValue;
        set
        {
            if (_runtimeValue == value) return;

            _runtimeValue = value;
            OnValueChanged?.Invoke(_runtimeValue);
        }
    }
}

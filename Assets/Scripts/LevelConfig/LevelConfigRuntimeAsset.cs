using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/New Level Config")]
public class LevelConfigRuntimeAsset : ScriptableObject
{
    //Standard/ColorPicker Runtime
    private LevelConfig _runtimeValue;
    public event Action<LevelConfig> OnValueChanged;
    public LevelConfig Value
    {
        get => _runtimeValue;
        set
        {
            if (_runtimeValue == value) return;

            _runtimeValue = value;
            OnValueChanged?.Invoke(_runtimeValue);
        }
    }
    
    // Sandbox runtime
    private SandboxModeConfig _sandboxRuntimeValue;
    public event Action<SandboxModeConfig> OnSandboxValueChanged;
    public SandboxModeConfig SandboxValue
    {
        get => _sandboxRuntimeValue;
        set
        {
            if (_sandboxRuntimeValue == value) return;
            _sandboxRuntimeValue = value;
            OnSandboxValueChanged?.Invoke(_sandboxRuntimeValue);
        }
    }

    // Helper runtime color list for sandbox
    private List<Color> _runtimeSandboxColors;
    public void InitializeSandboxColors()
    {
        if (_sandboxRuntimeValue == null) return;
        _runtimeSandboxColors = new List<Color>(_sandboxRuntimeValue.ColorsToBeUsed?.Value ?? new List<Color>());
    }
    
    public List<Color> GetActiveSandboxColors()
    {
        if (_runtimeSandboxColors == null) InitializeSandboxColors();
        return _runtimeSandboxColors;
    }
    
    public void SetSandboxColor(int index, Color newColor)
    {
        if (_runtimeSandboxColors == null) InitializeSandboxColors();
        if (index < 0 || index >= _runtimeSandboxColors.Count) return;
        _runtimeSandboxColors[index] = newColor;
    }

    public void AddSandboxColor(Color color)
    {
        if (_runtimeSandboxColors == null) InitializeSandboxColors();
        _runtimeSandboxColors.Add(color);
    }
}

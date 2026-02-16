using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/New Level Config")]
public class LevelConfigRuntimeAsset : ScriptableObject
{
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
}

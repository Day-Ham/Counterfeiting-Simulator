using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/New Level Config")]
public class LevelConfigRuntimeAsset : ScriptableObject
{
    private LevelConfig runtimeValue;
    public event Action<LevelConfig> OnValueChanged;

    public LevelConfig Value
    {
        get => runtimeValue;
        set
        {
            if (runtimeValue == value) return;

            runtimeValue = value;
            OnValueChanged?.Invoke(runtimeValue);
        }
    }
}

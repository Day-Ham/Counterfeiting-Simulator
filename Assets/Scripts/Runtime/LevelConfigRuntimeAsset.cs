using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Runtime Assets/Level Config")]
public class LevelConfigRuntimeAsset : ScriptableObject
{
    private TargetTextureTemplate runtimeValue;

    public event Action<TargetTextureTemplate> OnValueChanged;

    public TargetTextureTemplate Value
    {
        get => runtimeValue;
        set
        {
            if (runtimeValue == value)
            {
                return;
            }

            runtimeValue = value;
            OnValueChanged?.Invoke(runtimeValue);
        }
    }
}

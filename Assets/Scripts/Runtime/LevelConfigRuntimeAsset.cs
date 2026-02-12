using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Runtime Assets/Level Config")]
public class LevelConfigRuntimeAsset : ScriptableObject
{
    private LevelConfigScriptableObject runtimeValue;

    public event Action<LevelConfigScriptableObject> OnValueChanged;

    public LevelConfigScriptableObject Value
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

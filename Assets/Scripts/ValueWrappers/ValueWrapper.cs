using System;
using UnityEngine;

/// <summary>
/// this will be used instead of a hard set variable eg. 100 required experience, constants, gravity
/// </summary>

public class ValueWrapper<T> : ScriptableObject
{
    [Header("Stored Value")]
    [SerializeField] protected T value;
    
    private T runtimeValue;
    
    public event Action<T> OnValueChanged;

    public T Value
    {
        get => value;
        set
        {
            if (Equals(this.value, value))
                return;

            this.value = value;
            OnValueChanged?.Invoke(value);
        }
    }
    
    protected virtual void OnEnable()
    {
        // Reset runtime value when entering play mode
        runtimeValue = value;
    }

    public void SetObjectValue(object newValue)
    {
        if (newValue is T castValue)
        {
            Value = castValue; // use property so event fires
        }
        else
        {
            Debug.LogWarning(
                $"[ValueWrapper] Tried to assign {newValue?.GetType()} to {typeof(T)}"
            );
        }
    }
}

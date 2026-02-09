using UnityEngine;

/// <summary>
/// this will be used instead of a hard set variable eg. 100 required experience, constants, gravity
/// </summary>

public class ValueWrapper<T> : ScriptableObject
{
    [Header("Stored Value")]
    [SerializeField] protected T value;
    
    public T Value
    {
        get => value;
        set => this.value = value;
    }

    public void SetObjectValue(object newValue)
    {
        if (newValue is T castValue)
        {
            value = castValue;
        }
        else
        {
            Debug.LogWarning
            (
                $"[ValueWrapper] Tried to assign {newValue?.GetType()} to {typeof(T)}"
            );
        }
    }
}

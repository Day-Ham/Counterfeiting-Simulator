using System;
using UnityEngine;

public abstract class BaseEventT2<T1, T2> : ScriptableObject
{
    private Action<T1, T2> _listeners;

    public void Register(Action<T1, T2> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<T1, T2> listener)
    {
        _listeners -= listener;
    }

    public void Raise(T1 argument1, T2 argument2)
    {
        _listeners?.Invoke(argument1, argument2);
    }
}

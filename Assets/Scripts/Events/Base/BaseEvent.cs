using System;
using UnityEngine;

public class BaseEvent : ScriptableObject
{
    private Action _listeners;

    public void Register(Action listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action listener)
    {
        _listeners -= listener;
    }

    public void Raise()
    {
        _listeners?.Invoke();
    }
}

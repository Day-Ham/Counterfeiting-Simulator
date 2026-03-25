using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolEvent", menuName = "Events/BoolEvent")]
public class BoolEvent : ScriptableObject
{
    private Action<bool> _listeners;

    public void Register(Action<bool> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<bool> listener)
    {
        _listeners -= listener;
    }

    public void Raise(bool value)
    {
        _listeners?.Invoke(value);
    }
}

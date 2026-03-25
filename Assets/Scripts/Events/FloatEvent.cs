using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatEvent", menuName = "Events/FloatEvent")]
public class FloatEvent : ScriptableObject
{
    private Action<float> _listeners;

    public void Register(Action<float> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<float> listener)
    {
        _listeners -= listener;
    }

    public void Raise(float value)
    {
        _listeners?.Invoke(value);
    }
}

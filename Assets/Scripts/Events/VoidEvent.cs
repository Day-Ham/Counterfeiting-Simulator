using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidEvent", menuName = "Events/VoidEvent")]
public class VoidEvent : ScriptableObject
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

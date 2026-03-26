using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntEvent", menuName = "Events/IntEvent")]
public class IntEvent : ScriptableObject
{
    private Action<int> _listeners;
    
    public void Register(Action<int> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<int> listener)
    {
        _listeners -= listener;
    }

    public void Raise(int index)
    {
        _listeners?.Invoke(index);
    }
}

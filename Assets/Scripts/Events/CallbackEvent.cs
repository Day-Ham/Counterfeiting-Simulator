using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CallbackEvent", menuName = "Events/CallbackEvent")]
public class CallbackEvent : ScriptableObject
{
    private Action<Action> _listeners;

    public void Register(Action<Action> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<Action> listener)
    {
        _listeners -= listener;
    }

    public void Raise(Action onComplete = null)
    {
        _listeners?.Invoke(onComplete);
    }
}

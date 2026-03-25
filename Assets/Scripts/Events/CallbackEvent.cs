using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CallbackEvent", menuName = "Events/CallbackEvent")]
public class CallbackEvent : ScriptableObject
{
    private event Action<Action> Listeners;

    public void Register(Action<Action> listener)
    {
        Listeners += listener;
    }

    public void Unregister(Action<Action> listener)
    {
        Listeners -= listener;
    }

    public void Raise(Action onComplete = null)
    {
        Listeners?.Invoke(onComplete);
    }
}

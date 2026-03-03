using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Events")]
public class VoidEvent : ScriptableObject
{
    private Action listeners;

    public void Register(Action listener)
    {
        listeners += listener;
    }

    public void Unregister(Action listener)
    {
        listeners -= listener;
    }

    public void Raise()
    {
        listeners?.Invoke();
    }
}

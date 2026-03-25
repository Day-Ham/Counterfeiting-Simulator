using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Events/Selected Color Event")]
public class SelectedColorEvent : ScriptableObject
{
    private Action<int, Color> _listeners;

    public void Register(Action<int, Color> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<int, Color> listener)
    {
        _listeners -= listener;
    }

    public void Raise(int index, Color color)
    {
        _listeners?.Invoke(index, color);
    }
}

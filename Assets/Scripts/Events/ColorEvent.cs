using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/ColorPickerEvent")]
public class ColorEvent : ScriptableObject
{
    private Action<Color> _openListeners;

    public void RegisterColor(Action<Color> listener)
    {
        _openListeners += listener;
    }

    public void UnregisterColor(Action<Color> listener)
    {
        _openListeners -= listener;
    }

    public void Raise(Color color)
    {
        _openListeners?.Invoke(color);
    }
}

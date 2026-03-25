using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/ColorPickerEvent")]
public class OpenColorPickerEvent : ScriptableObject
{
    private Action<Color> _openListeners;
    private Action<bool> _toggleListeners;
    
    public void RegisterColor(Action<Color> listener)
    {
        _openListeners += listener;
    }

    public void RegisterToggleBool(Action<bool> listener)
    {
        _toggleListeners += listener;
    }
    
    public void UnregisterColor(Action<Color> listener)
    {
        _openListeners -= listener;
    }

    public void UnregisterToggleBool(Action<bool> listener)
    {
        _toggleListeners -= listener;
    }
    
    public void Raise(Color color)
    {
        _openListeners?.Invoke(color);

        _toggleListeners?.Invoke(true);
    }

    public void RaiseClosed()
    {
        _toggleListeners?.Invoke(false);
    }
}

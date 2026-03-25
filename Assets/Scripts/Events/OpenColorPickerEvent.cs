using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/ColorPickerEvent")]
public class OpenColorPickerEvent : ScriptableObject
{
    public event Action<Color> OnColorPickerOpened;
    public event Action<bool> OnColorPickerToggle;
    
    public void Raise(Color color)
    {
        OnColorPickerOpened?.Invoke(color);
        OnColorPickerToggle?.Invoke(true);
    }
    
    public void RaiseClosed()
    {
        OnColorPickerToggle?.Invoke(false);
    }
}

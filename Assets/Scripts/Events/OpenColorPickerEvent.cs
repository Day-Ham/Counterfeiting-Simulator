using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Color Picker Event")]
public class OpenColorPickerEvent : ScriptableObject
{
    public event Action<Color> OnColorPickerOpened;

    public void Raise(Color color)
    {
        OnColorPickerOpened?.Invoke(color);
    }
}

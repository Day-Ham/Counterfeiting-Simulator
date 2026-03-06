using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Events/Color Picked Event")]
public class ColorPickedEvent : ScriptableObject
{
    public event Action<int, Color> OnColorPicked;

    public void Raise(int index, Color color)
    {
        OnColorPicked?.Invoke(index, color);
    }
}

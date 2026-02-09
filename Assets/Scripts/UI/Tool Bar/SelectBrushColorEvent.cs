using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Drawing/Select Brush Color Event")]
public class SelectBrushColorEvent : ScriptableObject
{
    public Action<Color> OnColorSelected;

    public void Raise(Color color)
    {
        OnColorSelected?.Invoke(color);
    }
}

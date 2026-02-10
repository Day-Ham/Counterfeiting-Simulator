using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Drawing/Select Brush Color Event")]
public class SelectBrushColorEvent : ScriptableObject
{
    public Action<Color> OnColorSelected;
    public Action OnEraseSelected;

    public void Raise(Color color)
    {
        OnColorSelected?.Invoke(color);
    }
    
    public void RaiseErase()
    {
        OnEraseSelected?.Invoke();
    }
}

using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/Select Brush Color Event")]
public class SelectBrushColorEvent : ScriptableObject
{
    public event Action<int> OnColorSelected;
    public Action OnEraseSelected;

    public void Raise(int colorIndex)
    {
        OnColorSelected?.Invoke(colorIndex);
    }
    
    public void RaiseErase()
    {
        OnEraseSelected?.Invoke();
    }
}

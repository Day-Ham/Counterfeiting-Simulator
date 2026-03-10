using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/Select Brush Color Event")]
public class SelectBrushColorEvent : ScriptableObject
{
    public event Action<int> OnColorSelected;
    public Action OnEraseSelected;
    
    public int CurrentSelectedIndex { get; private set; } = -1;

    public void Raise(int colorIndex)
    {
        CurrentSelectedIndex = colorIndex;
        OnColorSelected?.Invoke(colorIndex);
    }
    
    public void RaiseErase()
    {
        CurrentSelectedIndex = -1; // no selection
        OnEraseSelected?.Invoke();
    }
}

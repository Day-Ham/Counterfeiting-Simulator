using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Events/Selected Color Event")]
public class SelectedColorEvent : ScriptableObject
{
    public event Action<int, Color> OnColorPicked;

    public void Raise(int index, Color color)
    {
        OnColorPicked?.Invoke(index, color);
    }
}

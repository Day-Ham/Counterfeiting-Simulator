using System;
using UnityEngine;

public abstract class ConfigRuntime : ScriptableObject
{
    public abstract event Action OnValueChanged;

    public abstract System.Collections.Generic.List<Color> GetActiveColors();
    public abstract int UndoLimit { get; }
    
    public abstract bool HasValue { get; }
}

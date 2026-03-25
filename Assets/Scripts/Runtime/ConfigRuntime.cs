using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigRuntime : ScriptableObject
{
    public abstract event Action OnValueChanged;
    public abstract List<Color> GetActiveColors();
    public abstract int UndoLimit { get; }
    public abstract bool HasValue { get; }
    public virtual bool UseSnapping => true;
}

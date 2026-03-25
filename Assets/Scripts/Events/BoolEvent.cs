using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolEvent", menuName = "Events/BoolEvent")]
public class BoolEvent : ScriptableObject
{
    public event Action<bool> OnRaised;

    public void Raise(bool value)
    {
        OnRaised?.Invoke(value);
    }
}

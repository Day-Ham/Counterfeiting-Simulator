using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatEvent", menuName = "Events/FloatEvent")]
public class FloatEvent : ScriptableObject
{
    public Action<float> OnRaised;

    public void Raise(float value)
    {
        OnRaised?.Invoke(value);
    }
}

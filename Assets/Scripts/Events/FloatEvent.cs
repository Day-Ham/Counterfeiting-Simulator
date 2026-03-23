using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Float Event")]
public class FloatEvent : ScriptableObject
{
    public Action<float> OnRaised;

    public void Raise(float value)
    {
        OnRaised?.Invoke(value);
    }
}

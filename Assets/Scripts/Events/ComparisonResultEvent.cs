using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/ComparisonEvent")]
public class ComparisonResultEvent : ScriptableObject
{
    private Action<float, float, float> _listeners;

    public void Register(Action<float, float, float> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<float, float, float> listener)
    {
        _listeners -= listener;
    }

    public void Raise(float similarity, float firstTwoDigits, float lastTwoDigits)
    {
        _listeners?.Invoke(similarity, firstTwoDigits, lastTwoDigits);
    }
}

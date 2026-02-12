using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/ComparisonEvent")]
public class ComparisonResultEvent : ScriptableObject
{
    public event Action<float, float, float> OnRaised;

    public void Raise(float similarity, float firstTwoDigits, float lastTwoDigits)
    {
        OnRaised?.Invoke(similarity, firstTwoDigits, lastTwoDigits);
    }
}

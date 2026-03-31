using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/ComparisonEvent")]
public class ComparisonResultEvent : ScriptableObject
{
    private Action<ComparisonResultStruct> _listeners;

    public void Register(Action<ComparisonResultStruct> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<ComparisonResultStruct> listener)
    {
        _listeners -= listener;
    }

    public void Raise(ComparisonResultStruct result)
    {
        _listeners?.Invoke(result);
    }
}

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Events")]
public class VoidEvent : ScriptableObject
{
    public Action OnRaised;

    public void Raise()
    {
        OnRaised?.Invoke();
    }
}

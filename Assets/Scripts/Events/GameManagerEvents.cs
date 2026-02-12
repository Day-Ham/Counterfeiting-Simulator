using UnityEngine;
using System;
using DaeHanKim.ThisIsTotallyADollar.Core;

[CreateAssetMenu(menuName = "Events/GameManagerEvent")]
public class GameManagerEvents : ScriptableObject
{
    public event Action OnRaised;

    public void Raise()
    {
        OnRaised?.Invoke();
    }
}

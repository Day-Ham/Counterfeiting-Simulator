using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavedEvent", menuName = "Events/SavedEvent")]
public class AuctionSavedEvent : ScriptableObject
{
    private Action<AuctionSavedData> _listeners;

    public void Register(Action<AuctionSavedData> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<AuctionSavedData> listener)
    {
        _listeners -= listener;
    }

    public void Raise(AuctionSavedData data)
    {
        _listeners?.Invoke(data);
    }
}

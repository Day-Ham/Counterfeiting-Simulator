using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ByteEvent", menuName = "Events/ByteEvent")]
public class ByteArrayEvent : ScriptableObject
{
    private Action<byte[]> _listeners;

    public void Register(Action<byte[]> listener)
    {
        _listeners += listener;
    }

    public void Unregister(Action<byte[]> listener)
    {
        _listeners -= listener;
    }

    public void Raise(byte[] data)
    {
        _listeners?.Invoke(data);
    }
}

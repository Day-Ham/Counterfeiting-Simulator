using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Assets/Sprite")]
public class SpriteContainerRuntimeAsset : ScriptableObject
{
    public Sprite Sprite
    {
        get => _sprite;
        set
        {
            if (_sprite == value)
                return;

            _sprite = value;
            OnSpriteChanged?.Invoke(value);
        }
    }

    public event Action<Sprite> OnSpriteChanged;

    Sprite _sprite;
}

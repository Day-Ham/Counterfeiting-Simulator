using UnityEngine;

/// <summary>
/// Automatically sets the SpriteRenderer's sprite to the sprite inside the sprite container.
/// </summary>
[ExecuteAlways]
public class SpriteContainerToSpriteRendererBinder : MonoBehaviour
{
    [SerializeField] SpriteContainerRuntimeAsset _spriteContainerRuntimeAsset;
    [SerializeField] SpriteRenderer _spriteRenderer;

    bool _isBindingActive;

    void OnEnable()
    {
        if (_spriteRenderer == null || _spriteContainerRuntimeAsset == null)
            return;

        _isBindingActive = true;
        _spriteRenderer.sprite = _spriteContainerRuntimeAsset.Sprite;
        _spriteContainerRuntimeAsset.OnSpriteChanged += OnSpriteChanged;
    }

    void OnDisable()
    {
        if (_spriteRenderer == null)
            return;

        _isBindingActive = false;
        _spriteContainerRuntimeAsset.OnSpriteChanged -= OnSpriteChanged;
    }

    void OnSpriteChanged(Sprite sprite)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = sprite;
        }
    }

    void OnValidate()
    {
        if (_isBindingActive)
            return;

        OnEnable();
    }
}

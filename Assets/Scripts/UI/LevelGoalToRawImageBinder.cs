using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoalToRawImageBinder : MonoBehaviour
{
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntime;
    [SerializeField] private List<RawImage> _rawImages;

    private void OnEnable()
    {
        if (_levelConfigRuntime == null) return;

        _levelConfigRuntime.OnValueChanged += OnLevelConfigChanged;

        if (_levelConfigRuntime.Value != null)
        {
            ApplyTexture(_levelConfigRuntime.Value.GoalTexture);
        }
    }

    private void OnDisable()
    {
        if (_levelConfigRuntime != null)
        {
            _levelConfigRuntime.OnValueChanged -= OnLevelConfigChanged;
        }
    }

    private void OnLevelConfigChanged(TargetTextureTemplate config)
    {
        if (config == null) return;

        ApplyTexture(config.GoalTexture);
    }

    private void ApplyTexture(Texture texture)
    {
        if (texture == null) return;

        foreach (var rawImage in _rawImages)
        {
            if (rawImage != null)
            {
                rawImage.texture = texture;
            }
        }
    }
}

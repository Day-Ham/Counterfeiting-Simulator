using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoalToRawImageBinder : MonoBehaviour
{
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntime;
    [SerializeField] private List<RawImage> _rawImages;

    private void OnEnable()
    {
        _levelConfigRuntime.OnValueChanged += OnLevelConfigChanged;

        ApplyTexture(_levelConfigRuntime.Value.GoalTexture);
    }

    private void OnDisable()
    {
        _levelConfigRuntime.OnValueChanged -= OnLevelConfigChanged;
    }

    private void OnLevelConfigChanged(LevelConfig newConfig)
    {
        if (newConfig == null) return;

        ApplyTexture(newConfig.GoalTexture);
    }

    private void ApplyTexture(TextureValueWrapper textureValue)
    {
        if (textureValue == null) return;

        foreach (var rawImage in _rawImages)
        {
            if (rawImage != null)
            {
                rawImage.texture = textureValue.Value;
            }
        }
    }
}

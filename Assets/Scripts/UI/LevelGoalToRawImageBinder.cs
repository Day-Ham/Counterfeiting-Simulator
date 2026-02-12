using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoalToRawImageBinder : MonoBehaviour
{
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntime;
    [SerializeField] private List<RawImage> _rawImages;

    private TextureValueWrapper currentGoalTexture;

    private void OnEnable()
    {
        if (_levelConfigRuntime == null) return;
        
        _levelConfigRuntime.OnValueChanged += OnLevelConfigChanged;

        if (_levelConfigRuntime.Value != null)
        {
            BindGoalTexture(_levelConfigRuntime.Value);
        }
    }

    private void OnDisable()
    {
        if (_levelConfigRuntime != null)
        {
            _levelConfigRuntime.OnValueChanged -= OnLevelConfigChanged;
        }

        UnbindGoalTexture();
    }

    private void OnLevelConfigChanged(LevelConfigScriptableObject config)
    {
        BindGoalTexture(config);
    }

    private void BindGoalTexture(LevelConfigScriptableObject config)
    {
        if (config == null || config.GoalTexture == null)
        {
            return;
        }

        UnbindGoalTexture();

        currentGoalTexture = config.GoalTexture;
        currentGoalTexture.OnValueChanged += ApplyTexture;

        ApplyTexture(currentGoalTexture.Value);
    }

    private void UnbindGoalTexture()
    {
        if (currentGoalTexture == null) return;
        
        currentGoalTexture.OnValueChanged -= ApplyTexture;
        currentGoalTexture = null;
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

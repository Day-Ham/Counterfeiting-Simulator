using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoalToRawImageBinder : MonoBehaviour
{
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntime;
    [SerializeField] private List<RawImage> _rawImages;
    [SerializeField] private List<Image> _drawingBackground; 

    private void Start()
    {
        if (_levelConfigRuntime == null || _levelConfigRuntime.Value == null)
            return;

        ApplyTexture(_levelConfigRuntime.Value.TargetTexture);
    }

    private void ApplyTexture(TextureValueWrapper textureValue)
    {
        if (textureValue == null || textureValue.Value == null)
            return;

        Texture texture = textureValue.Value;

        foreach (var rawImage in _rawImages)
        {
            if (rawImage != null)
            {
                rawImage.texture = texture;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoalToRawImageBinder : MonoBehaviour
{
    [SerializeField] private LevelConfigRuntimeAsset levelConfigRuntime;
    [SerializeField] private List<RawImage> rawImages;
    [SerializeField] private List<Image> drawingBackground; 

    private void Start()
    {
        if (levelConfigRuntime == null || levelConfigRuntime.Value == null)
            return;

        ApplyTexture(levelConfigRuntime.Value.TargetTexture);
        ApplyBackgroundColor(levelConfigRuntime.Value.ColorBackgroundDraw);
    }

    private void ApplyTexture(TextureValueWrapper textureValue)
    {
        if (textureValue == null || textureValue.Value == null)
            return;

        Texture texture = textureValue.Value;

        foreach (var rawImage in rawImages)
        {
            if (rawImage != null)
            {
                rawImage.texture = texture;
            }
        }
    }

    private void ApplyBackgroundColor(ColorDataValue colorData)
    {
        if(colorData == null)
            return;
        
        Color backgroundColor  = colorData.Value;

        foreach (var drawingBackgroundImages in drawingBackground)
        {
            if (drawingBackgroundImages != null)
            {
                drawingBackgroundImages.color = backgroundColor;
            }
        }
    }
}

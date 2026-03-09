using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Level/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public LevelGameMode LevelGameMode;
    public CanvasTemplateValue CanvasTemplate;
    public ColorDataValue ColorBackgroundDraw;
    public GameObjectListValue ColorBlobs;
    public ColorDataListValue ColorsToBeUsed;
    public ColorDataListValue WhiteColors;
    public TextureValueWrapper TargetTexture;
    
    public LevelGameMode GameMode => LevelGameMode;
    
    private List<Color> _runtimeWhiteColors;
    
    public void InitializeRuntimeWhiteColors()
    {
        _runtimeWhiteColors = new List<Color>(WhiteColors.Value);
    }

    public List<Color> GetActiveColors()
    {
        if (LevelGameMode == LevelGameMode.ColorPicker) return _runtimeWhiteColors;

        return ColorsToBeUsed.Value;
    }

    public void SetWhiteColor(int index, Color newColor)
    {
        if (_runtimeWhiteColors == null) return;
        
        if (index >= 0 && index < _runtimeWhiteColors.Count)
        {
            _runtimeWhiteColors[index] = newColor;
        }
    }
}

public enum LevelGameMode
{
    Standard,
    ColorPicker
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Level/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [Header("Core Level Data")]
    public LevelGameMode GameMode;
    public CanvasTemplateValue CanvasTemplate;
    public ColorDataValue ColorBackgroundDraw;
    public ColorDataListValue ColorsToBeUsed;
    public ColorDataListValue WhiteColors;
    public TextureValueWrapper TargetTexture;
    
    [Header("Gameplay Limits")]
    public IntValue UndoLimit;
    public IntValue SnapTolerance;

    [Header("Universal Data")] 
    public GameObjectListValue ColorBlobs;
    
    private List<Color> _runtimeWhiteColors;
    
    public void InitializeRuntimeWhiteColors()
    {
        _runtimeWhiteColors = new List<Color>(WhiteColors?.Value ?? new List<Color>());
    }

    public List<Color> GetActiveColors()
    {
        return GameMode == LevelGameMode.ColorPicker ? _runtimeWhiteColors : ColorsToBeUsed?.Value;
    }

    public void SetWhiteColor(int index, Color newColor)
    {
        if (_runtimeWhiteColors == null || index < 0 || index >= _runtimeWhiteColors.Count) return;

        _runtimeWhiteColors[index] = SnapColorIfNeeded(newColor);
    }
    
    private Color SnapColorIfNeeded(Color color)
    {
        if (ColorsToBeUsed?.Value != null)
        {
            return ColorMatchUtils.SnapPerChannelClosest(color, ColorsToBeUsed.Value, SnapTolerance.Value);
        }

        return color;
    }
}

public enum LevelGameMode
{
    Standard,
    ColorPicker
}

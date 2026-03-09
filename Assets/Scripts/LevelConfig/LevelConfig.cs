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
    
    [Header("Other Configs")]
    [SerializeField] private ColorMatcher colorMatcher;
    public ColorMatcher ColorMatcher => colorMatcher;
    
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

        if (index < 0 || index >= _runtimeWhiteColors.Count) return;

        if (colorMatcher)
        {
            newColor = colorMatcher.SnapPerChannel(newColor, ColorsToBeUsed.Value);
        }

        _runtimeWhiteColors[index] = newColor;
        
        int r = Mathf.RoundToInt(newColor.r * 255f);
        int g = Mathf.RoundToInt(newColor.g * 255f);
        int b = Mathf.RoundToInt(newColor.b * 255f);

        int sr = Mathf.RoundToInt(_runtimeWhiteColors[index].r * 255f);
        int sg = Mathf.RoundToInt(_runtimeWhiteColors[index].g * 255f);
        int sb = Mathf.RoundToInt(_runtimeWhiteColors[index].b * 255f);

        Debug.Log($"PlayerColor: RGB({r}, {g}, {b}) → Snapped: RGB({sr}, {sg}, {sb})");
    }
}

public enum LevelGameMode
{
    Standard,
    ColorPicker
}

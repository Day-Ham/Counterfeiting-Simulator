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

    public ColorDataListValue GetActiveColors()
    {
        return LevelGameMode == LevelGameMode.ColorPicker ? WhiteColors : ColorsToBeUsed;
    }

    public void SetWhiteColor(int index, Color newColor)
    {
        if (WhiteColors != null && index >= 0 && index < WhiteColors.Value.Count)
        {
            WhiteColors.Value[index] = newColor;
        }
    }

    // Reset all white colors to pure white
    public void ResetWhiteColors()
    {
        if (WhiteColors != null)
        {
            for (int i = 0; i < WhiteColors.Value.Count; i++)
            {
                WhiteColors.Value[i] = Color.white;
            }
        }
    }
}

public enum LevelGameMode
{
    Standard,
    ColorPicker
}

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
}

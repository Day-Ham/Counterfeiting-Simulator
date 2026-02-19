using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Level/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public CanvasTemplateValue CanvasTemplate;
    public ColorDataValue ColorBackgroundDraw;
    public GameObjectListValue ColorBlobs;
    public ColorDataListValue ColorsToBeUsed;
    public TextureValueWrapper TargetTexture;
}

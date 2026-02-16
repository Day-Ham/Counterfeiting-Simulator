using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Level/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public CanvasTemplateValue CanvasTemplate;
    public TextureValueWrapper GoalTexture;
    public ColorDataListValue LevelColors;
    public GameObjectListValue LevelColorBlobs;
}

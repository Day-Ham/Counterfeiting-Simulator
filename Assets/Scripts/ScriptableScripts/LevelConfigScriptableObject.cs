using UnityEngine;

[CreateAssetMenu(menuName = "Level/Level Config")]
public class LevelConfigScriptableObject : ScriptableObject
{
    [Header("Goal")]
    public Texture GoalTexture;
}

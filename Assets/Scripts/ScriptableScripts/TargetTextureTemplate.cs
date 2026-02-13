using UnityEngine;

[CreateAssetMenu(menuName = "Texture/Level Config")]
public class TargetTextureTemplate : ScriptableObject
{
    [Header("Goal")]
    public Texture GoalTexture;
}

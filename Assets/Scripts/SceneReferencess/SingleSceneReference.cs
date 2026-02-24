using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SingeSceneDatabase", menuName = "Scenes/SingleSceneDatabase")]
public class SingleSceneReference : SceneReference
{
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    public string sceneName;
    public string SceneName => sceneName;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif
}

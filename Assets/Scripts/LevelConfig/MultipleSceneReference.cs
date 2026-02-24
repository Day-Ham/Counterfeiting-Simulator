using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MultipleSceneDatabase", menuName = "Scenes/MultipleSceneDatabase")]
public class MultipleSceneReference : SceneReference
{
    [SerializeField] private List<SceneEntry> scenes = new List<SceneEntry>();

    public IReadOnlyList<SceneEntry> Scenes => scenes;

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var level in scenes)
        {
            if (level.sceneAsset != null)
            {
                level.sceneName = level.sceneAsset.name;
            }
        }
    }
#endif
}

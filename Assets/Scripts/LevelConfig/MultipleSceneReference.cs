using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Level/LevelSceneDatabase")]
public class MultipleSceneReference : ScriptableObject
{
    [System.Serializable]
    public class SceneEntry
    {
#if UNITY_EDITOR
        public SceneAsset sceneAsset;
#endif
        public string sceneName;
    }

    [SerializeField] private List<SceneEntry> levels = new List<SceneEntry>();

    public IReadOnlyList<SceneEntry> Levels => levels;

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var level in levels)
        {
            if (level.sceneAsset != null)
            {
                level.sceneName = level.sceneAsset.name;
            }
        }
    }
#endif
}

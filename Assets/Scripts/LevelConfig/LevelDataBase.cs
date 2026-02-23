using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/Level Database")]
public class LevelDataBase : ScriptableObject
{
    [System.Serializable]
    public class LevelEntry
    {
#if UNITY_EDITOR
        public SceneAsset sceneAsset;
#endif
        public string sceneName;
    }

    [SerializeField] private List<LevelEntry> levels = new List<LevelEntry>();

    public IReadOnlyList<LevelEntry> Levels => levels;

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

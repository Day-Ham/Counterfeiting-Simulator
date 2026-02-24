using UnityEditor;
using UnityEngine;

public class SceneReference : ScriptableObject
{
    [System.Serializable]
    public class SceneEntry
    {
#if UNITY_EDITOR
        public SceneAsset sceneAsset;
#endif
        public string sceneName;
    }
}

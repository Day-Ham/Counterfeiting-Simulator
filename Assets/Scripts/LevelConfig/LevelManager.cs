using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private NewLevelConfigRuntimeAsset LevelRuntime;
    [SerializeField] private LevelConfig LevelToLoad;

    private void Awake()
    {
        LevelRuntime.Value = LevelToLoad;
    }
}

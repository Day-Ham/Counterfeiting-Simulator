using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfigRuntimeAsset LevelRuntime;
    [SerializeField] private LevelConfig LevelToLoad;

    private void Awake()
    {
        LevelRuntime.Value = LevelToLoad;
    }
}

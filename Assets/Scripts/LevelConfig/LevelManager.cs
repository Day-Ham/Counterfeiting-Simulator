using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfig _levelToLoad;
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;

    private void Awake()
    {
        _levelConfigRuntimeAsset.Value = _levelToLoad;
    }
}

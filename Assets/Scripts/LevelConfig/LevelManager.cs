using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfig _levelToLoad;
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    
    [Header("Prefabs to Instantiate")]
    [SerializeField] private GameObjectListValue _gameObjectListValueToSpawn;

    private void Awake()
    {
        _levelConfigRuntimeAsset.Value = _levelToLoad;
        
        ColorDataListValue runtimeWhiteColors = ScriptableObject.CreateInstance<ColorDataListValue>();
        runtimeWhiteColors.Value = new List<Color>(_levelToLoad.WhiteColors.Value);

        LevelConfig runtimeLevel = ScriptableObject.CreateInstance<LevelConfig>();
        runtimeLevel.WhiteColors = runtimeWhiteColors;
        
        SpawnPrefabs();
    }
    
    private void SpawnPrefabs()
    {
        foreach (GameObject listGameObject in _gameObjectListValueToSpawn.Value)
        {
            Instantiate(listGameObject);
        }
    }
}

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
        
        if (_levelToLoad.WhiteColors != null && _levelToLoad.WhiteColors.Value.Count > 0)
        {
            var runtimeWhiteColors = ScriptableObject.CreateInstance<ColorDataListValue>();
            runtimeWhiteColors.Value = new List<Color>(_levelToLoad.WhiteColors.Value);
            
            var runtimeLevel = ScriptableObject.CreateInstance<LevelConfig>();
            runtimeLevel.WhiteColors = runtimeWhiteColors;

            _levelConfigRuntimeAsset.Value = runtimeLevel;
        }
        
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

using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfig _levelToLoad;
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    
    [Header("Prefabs to Instantiate")]
    [SerializeField] private GameObjectListValue _gameObjectListValueToSpawn;
    
    private RuntimeWhiteColorData _runtimeWhiteLevel;

    private void Awake()
    {
        _levelConfigRuntimeAsset.Value = _levelToLoad;
        
        if (_levelToLoad.WhiteColors != null && _levelToLoad.WhiteColors.Value.Count > 0)
        {
            _runtimeWhiteLevel= new RuntimeWhiteColorData();

            if (_levelToLoad.WhiteColors != null)
            {
                _runtimeWhiteLevel.WhiteColors = new List<Color>(_levelToLoad.WhiteColors.Value);
            }
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

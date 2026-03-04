using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfig _levelToLoad;
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    
    [Header("Prefabs to Instantiate")]
    [SerializeField] private List<GameObject> _prefabsToSpawn;

    private void Awake()
    {
        _levelConfigRuntimeAsset.Value = _levelToLoad;
        SpawnPrefabs();
    }
    
    private void SpawnPrefabs()
    {
        foreach (GameObject prefab in _prefabsToSpawn)
        {
            Instantiate(prefab);
        }
    }
}

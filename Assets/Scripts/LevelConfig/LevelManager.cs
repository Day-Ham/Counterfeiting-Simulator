using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfigListValue levelConfigListValue;
    [SerializeField] private LevelConfigRuntimeAsset levelConfigRuntimeAsset;
    [SerializeField] private IntValue currentLevelIndexValue;
    
    [Header("Prefabs To Spawn")]
    [SerializeField] private GameObjectListValue prefabsToSpawn;

    private readonly List<GameObject> _spawnedObjects = new();
    private RuntimeWhiteColorData _runtimeWhiteLevel;
    
    public LevelConfig CurrentLevelConfig => levelConfigRuntimeAsset.Value;
    public RuntimeWhiteColorData RuntimeWhiteLevel => _runtimeWhiteLevel;
    
    private void OnEnable()
    {
        currentLevelIndexValue.OnValueChanged += HandleLevelChanged;
    }

    private void OnDisable()
    {
        currentLevelIndexValue.OnValueChanged -= HandleLevelChanged;
    }
    
    private void Awake()
    {
        if (levelConfigListValue.Value == null || levelConfigListValue.Value.Count == 0)
        {
            Debug.LogWarning("LevelManager: No levels assigned!");
            return;
        }
        
        ClampLevelIndex();
        HandleLevelChanged(currentLevelIndexValue.Value);
        SpawnObjects();
    }

    private void ClampLevelIndex()
    {
        if (currentLevelIndexValue.Value < 0 || currentLevelIndexValue.Value >= levelConfigListValue.Value.Count)
        {
            currentLevelIndexValue.Value = 0;
        }
    }
    
    private void HandleLevelChanged(int index)
    {
        if (index < 0 || index >= levelConfigListValue.Value.Count)
        {
            Debug.LogWarning($"Invalid level index {index}");
            return;
        }

        levelConfigRuntimeAsset.Value = levelConfigListValue.Value[index];

        InitializeWhiteColors(levelConfigRuntimeAsset.Value);

        ActivateLevelObjects();
    }

    private void SpawnObjects()
    {
        if (_spawnedObjects.Count > 0) return;
        
        foreach (GameObject prefab in prefabsToSpawn.Value)
        {
            GameObject instance = Instantiate(prefab);
            instance.SetActive(false);
            _spawnedObjects.Add(instance);
        }

        ActivateLevelObjects();
    }

    private void ActivateLevelObjects()
    {
        foreach (var spawnedGameObject in _spawnedObjects)
        {
            spawnedGameObject.SetActive(true);
        }
    }

    private void InitializeWhiteColors(LevelConfig level)
    {
        if (!level.WhiteColors || level.WhiteColors.Value.Count == 0)
        {
            _runtimeWhiteLevel = null;
            return;
        }

        _runtimeWhiteLevel = new RuntimeWhiteColorData(level.WhiteColors.Value);
    }
}

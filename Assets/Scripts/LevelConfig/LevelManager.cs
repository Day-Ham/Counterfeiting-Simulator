using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfigListValue levels;
    [SerializeField] private LevelConfigRuntimeAsset levelConfigRuntimeAsset;
    [SerializeField] private LevelManagerValue levelManagerValue;
    [SerializeField] private IntValue currentLevelIndex;
    
    [Header("Prefabs To Spawn")]
    [SerializeField] private GameObjectListValue prefabsToSpawn;

    private readonly List<GameObject> _spawnedObjects = new();
    private RuntimeWhiteColorData _runtimeWhiteLevel;

    public int CurrentLevelIndex => currentLevelIndex.Value;
    public int LevelCount => levels.Value.Count;
    public LevelConfig CurrentLevelConfig => levelConfigRuntimeAsset.Value;
    public RuntimeWhiteColorData RuntimeWhiteLevel => _runtimeWhiteLevel;
    
    private void Awake()
    {
        levelManagerValue.Value = this;

        if (levels.Value == null || levels.Value.Count == 0)
        {
            Debug.LogWarning("LevelManager: No levels assigned!");
            return;
        }

        ClampLevelIndex();
        SetCurrentLevel(currentLevelIndex.Value);
        SpawnObjects();
    }

    private void ClampLevelIndex()
    {
        if (currentLevelIndex.Value < 0 || currentLevelIndex.Value >= levels.Value.Count)
        {
            currentLevelIndex.Value = 0;
        }
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
            spawnedGameObject .SetActive(true);
        }
    }

    private void SetCurrentLevel(int index)
    {
        if (index < 0 || index >= levels.Value.Count)
        {
            Debug.LogWarning($"Invalid level index {index}");
            return;
        }

        currentLevelIndex.Value = index;
        levelConfigRuntimeAsset.Value = levels.Value[index];

        // Initialize white colors for the new level if level is colorPicker
        InitializeWhiteColors(levelConfigRuntimeAsset.Value);

        // Could also trigger events here if needed
        ActivateLevelObjects();
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

    public void LoadNextLevel()
    {
        if (currentLevelIndex.Value >= levels.Value.Count - 1) return;

        SetCurrentLevel(currentLevelIndex.Value + 1);
        ReloadLevel();
    }

    public void LoadPrevLevel()
    {
        if (currentLevelIndex.Value <= 0) return;

        SetCurrentLevel(currentLevelIndex.Value - 1);
        ReloadLevel();
    }

    public void ReloadLevel()
    {
        SceneManagerUtility.ReloadCurrentScene();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private LevelConfigListValue _levels;
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    [SerializeField] private LevelManagerValue _levelManagerValue;
    [SerializeField] private IntValue _currentLevelIndex;
    
    [Header("Prefabs To Spawn")]
    [SerializeField] private GameObjectListValue _prefabsToSpawn;

    private readonly List<GameObject> _spawnedObjects = new();
    private RuntimeWhiteColorData _runtimeWhiteLevel;

    public int CurrentLevelIndex => _currentLevelIndex.Value;
    public int LevelCount => _levels.Value.Count;
    public LevelConfig CurrentLevelConfig => _levelConfigRuntimeAsset.Value;
    public RuntimeWhiteColorData RuntimeWhiteLevel => _runtimeWhiteLevel;
    
    private void Awake()
    {
        _levelManagerValue.Value = this;

        if (_levels.Value == null || _levels.Value.Count == 0)
        {
            Debug.LogWarning("LevelManager: No levels assigned!");
            return;
        }

        ClampLevelIndex();
        ApplyLevel(_currentLevelIndex.Value);

        SpawnObjects();
    }

    private void ClampLevelIndex()
    {
        if (_currentLevelIndex.Value < 0 || _currentLevelIndex.Value >= _levels.Value.Count)
        {
            _currentLevelIndex.Value = 0;
        }
    }

    private void SpawnObjects()
    {
        foreach (GameObject prefab in _prefabsToSpawn.Value)
        {
            GameObject instance = Instantiate(prefab);
            instance.SetActive(false);
            _spawnedObjects.Add(instance);
        }

        ActivateLevelObjects();
    }

    private void ActivateLevelObjects()
    {
        foreach (var obj in _spawnedObjects)
        {
            obj.SetActive(true);
        }
    }

    private void ApplyLevel(int index)
    {
        _levelConfigRuntimeAsset.Value = _levels.Value[index];
        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);
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

    private void LoadLevel(int index)
    {
        if (index < 0 || index >= _levels.Value.Count) return;

        _currentLevelIndex.Value = index;
        ApplyLevel(index);
        ReloadLevel();
    }

    public void LoadNextLevel()
    {
        if (_currentLevelIndex.Value >= _levels.Value.Count - 1) return;

        LoadLevel(_currentLevelIndex.Value + 1);
    }

    public void LoadPrevLevel()
    {
        if (_currentLevelIndex.Value <= 0) return;

        LoadLevel(_currentLevelIndex.Value - 1);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

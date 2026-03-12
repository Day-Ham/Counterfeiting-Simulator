using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelConfig> _levels;
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    [SerializeField] private LevelManagerValue _levelManagerValue;
    [SerializeField] private IntValue _currentLevelIndexSO;
    
    [Header("Prefabs to Instantiate")]
    [SerializeField] private GameObjectListValue _gameObjectListValueToSpawn;

    public int CurrentLevelIndex => _currentLevelIndexSO.Value;
    public int LevelCount => _levels.Count;
    public LevelConfig CurrentLevelConfig => _levelConfigRuntimeAsset.Value;
    public RuntimeWhiteColorData RuntimeWhiteLevel => _runtimeWhiteLevel;
    
    private RuntimeWhiteColorData _runtimeWhiteLevel;
    private readonly List<GameObject> _spawnedObjects = new();
    private readonly Dictionary<GameObject, GameObject> _prefabToInstance = new();
    private int _currentLevelIndex => _currentLevelIndexSO.Value;

    private void Awake()
    {
        _levelManagerValue.Value = this;

        if (_levels.Count == 0)
        {
            Debug.LogWarning("No levels assigned to LevelManager!");
            return;
        }

        if (_currentLevelIndex < 0 || _currentLevelIndex >= _levels.Count)
        {
            _currentLevelIndexSO.Value = 0;
        }

        _levelConfigRuntimeAsset.Value = _levels[_currentLevelIndexSO.Value];
        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);
        
        foreach (GameObject prefab in _gameObjectListValueToSpawn.Value)
        {
            GameObject spawned = Instantiate(prefab);
            spawned.SetActive(false);
            _spawnedObjects.Add(spawned);
            _prefabToInstance[prefab] = spawned;
        }

        ActivateLevelObjects();
    }
    
    private void InitializeWhiteColors(LevelConfig level)
    {
        if (!level.WhiteColors || level.WhiteColors.Value.Count == 0)
        {
            _runtimeWhiteLevel = null;
            return;
        }

        _runtimeWhiteLevel = new RuntimeWhiteColorData
        {
            WhiteColors = new List<Color>(level.WhiteColors.Value)
        };
    }
    
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= _levels.Count)
        {
            return;
        }

        _currentLevelIndexSO.Value = index;
        _levelConfigRuntimeAsset.Value = _levels[index];
        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);

        ReloadLevel();
    }
    
    public void LoadNextLevel()
    {
        if (_currentLevelIndex >= _levels.Count - 1) return;

        _currentLevelIndexSO.Value++;
        _levelConfigRuntimeAsset.Value = _levels[_currentLevelIndexSO.Value];
        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);

        ReloadLevel();
    }

    public void LoadPrevLevel()
    {
        if (_currentLevelIndex <= 0) return;

        _currentLevelIndexSO.Value--;
        _levelConfigRuntimeAsset.Value = _levels[_currentLevelIndexSO.Value];
        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);

        ReloadLevel();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void ActivateLevelObjects()
    {
        foreach (var obj in _spawnedObjects) obj.SetActive(false);
        
        foreach (var prefab in _gameObjectListValueToSpawn.Value)
        {
            if (_prefabToInstance.TryGetValue(prefab, out var spawned))
            {
                spawned.SetActive(true);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelConfig> _levels;
    [SerializeField] private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    [SerializeField] private LevelManagerValue _levelManagerValue;
    
    [Header("Prefabs to Instantiate")]
    [SerializeField] private GameObjectListValue _gameObjectListValueToSpawn;

    private int _currentLevelIndex;
    private RuntimeWhiteColorData _runtimeWhiteLevel;
    private readonly List<GameObject> _spawnedObjects = new();
    private readonly Dictionary<GameObject, GameObject> _prefabToInstance = new();

    public int CurrentLevelIndex => _currentLevelIndex;
    public int LevelCount => _levels.Count;
    public LevelConfig CurrentLevelConfig => _levelConfigRuntimeAsset.Value;
    public RuntimeWhiteColorData RuntimeWhiteLevel => _runtimeWhiteLevel;

    private void Awake()
    {
        _levelManagerValue.Value = this;

        if (_levels.Count == 0)
        {
            Debug.LogWarning("No levels assigned to LevelManager!");
            return;
        }

        // Determine starting LevelConfig
        if (_levelConfigRuntimeAsset.Value == null)
        {
            _currentLevelIndex = 0;
            _levelConfigRuntimeAsset.Value = _levels[0];
        }
        else
        {
            _currentLevelIndex = _levels.IndexOf(_levelConfigRuntimeAsset.Value);
            if (_currentLevelIndex < 0) _currentLevelIndex = 0;
        }

        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);
        
        // Pre-instantiate prefabs once
        foreach (GameObject prefab in _gameObjectListValueToSpawn.Value)
        {
            GameObject spawned = Instantiate(prefab);
            spawned.SetActive(false); // deactivate by default
            _spawnedObjects.Add(spawned);
            _prefabToInstance[prefab] = spawned;
        }

        ActivateLevelObjects();
    }

    /// <summary>
    /// Load a specific level index
    /// </summary>
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= _levels.Count)
        {
            Debug.LogError($"Invalid Level Index: {index}");
            return;
        }

        _currentLevelIndex = index;
        _levelConfigRuntimeAsset.Value = _levels[index];

        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);
        ReloadScene();
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
    
    public void ReloadScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextLevel()
    {
        int nextIndex = _currentLevelIndex + 1;
        if (nextIndex >= _levels.Count) return;

        _currentLevelIndex = nextIndex;
        _levelConfigRuntimeAsset.Value = _levels[nextIndex];

        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);
        ReloadScene();
    }

    public void LoadPrevLevel()
    {
        int prevIndex = _currentLevelIndex - 1;
        if (prevIndex < 0) return;

        _currentLevelIndex = prevIndex;
        _levelConfigRuntimeAsset.Value = _levels[prevIndex];

        InitializeWhiteColors(_levelConfigRuntimeAsset.Value);
        ReloadScene();
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
}

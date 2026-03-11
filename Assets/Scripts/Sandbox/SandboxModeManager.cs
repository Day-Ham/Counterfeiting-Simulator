using System.Collections.Generic;
using UnityEngine;

public class SandboxModeManager : MonoBehaviour
{
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxModeConfig _sandboxConfig;

    [Header("Runtime Data")]
    [SerializeField] private SandboxConfigRuntimeAsset _runtimeAsset;
    
    [Header("Prefabs to Instantiate")]
    [SerializeField] private GameObjectListValue _gameObjectListValueToSpawn;

    private void Awake()
    {
        InitializeSandbox();
    }

    private void InitializeSandbox()
    {
        if (_sandboxConfig == null)
        {
            Debug.LogError("SandboxConfig not assigned!");
            return;
        }

        if (_runtimeAsset == null)
        {
            Debug.LogError("SandboxRuntimeAsset not assigned!");
            return;
        }

        _runtimeAsset.Value = _sandboxConfig;
        
        _runtimeAsset.Value.InitializeRuntimeColors();
        
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

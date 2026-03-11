using System.Collections.Generic;
using UnityEngine;

public class SandboxModeManager : MonoBehaviour
{
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxModeConfig _sandboxConfig;

    [Header("Runtime Data")]
    [SerializeField] private SandboxConfigRuntimeAsset _runtimeAsset;

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
    }

    public List<Color> GetActiveColors()
    {
        return _runtimeAsset.Value.GetActiveColors();
    }

    public void SetColor(int index, Color newColor)
    {
        _runtimeAsset.Value.SetColor(index, newColor);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SandboxModeManager : MonoBehaviour
{
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxModeConfig _sandboxConfig;

    [Header("Runtime Data")]
    [SerializeField] private LevelConfigRuntimeAsset _runtimeAsset;

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
            Debug.LogError("RuntimeAsset not assigned!");
            return;
        }
        
        _runtimeAsset.SandboxValue = _sandboxConfig;
        
        _runtimeAsset.InitializeSandboxColors();
        
        if (_sandboxConfig.CanvasTemplate != null)
        {
            _sandboxConfig.CanvasTemplate.Value.gameObject.SetActive(true);
        }
    }
    
    public List<Color> GetActiveColors()
    {
        return _runtimeAsset.GetActiveSandboxColors();
    }
    
    public void SetColor(int index, Color newColor)
    {
        _runtimeAsset.SetSandboxColor(index, newColor);
    }
}

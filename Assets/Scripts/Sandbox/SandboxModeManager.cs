using UnityEngine;

public class SandboxModeManager : MonoBehaviour
{
    [Header("Sandbox Settings")]
    [SerializeField] private SandboxModeConfig sandboxConfig;

    [Header("Runtime Data")]
    [SerializeField] private SandboxConfigRuntimeAsset runtimeAsset;

    private void Awake()
    {
        InitializeSandbox();
    }

    private void InitializeSandbox()
    {
        if (sandboxConfig == null)
        {
            Debug.LogError("SandboxConfig not assigned!");
            return;
        }

        if (runtimeAsset == null)
        {
            Debug.LogError("SandboxRuntimeAsset not assigned!");
            return;
        }

        runtimeAsset.Value = sandboxConfig;
        
        runtimeAsset.Value.InitializeRuntimeColors();
    }
}

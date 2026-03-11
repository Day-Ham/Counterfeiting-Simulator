using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SandboxCrayonSource : ICrayonSource
{
    private SandboxModeConfig _sandboxConfig;

    public SandboxCrayonSource(SandboxModeConfig config)
    {
        _sandboxConfig = config;
    }

    public void InitializeColors() => _sandboxConfig.InitializeRuntimeColors();

    public List<Color> GetActiveColors() => _sandboxConfig.GetActiveColors();

    public GameObjectListValue GetColorBlobs() => _sandboxConfig.ColorBlobs;
}

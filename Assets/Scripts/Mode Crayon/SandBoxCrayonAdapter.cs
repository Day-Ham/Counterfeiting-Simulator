using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SandboxCrayonAdapter", menuName = "Crayons/SandboxAdapter")]
public class SandBoxCrayonAdapter : GameModeCrayon
{
    [SerializeField] private SandboxModeConfig _sandboxConfig;

    public override void InitializeColors() => _sandboxConfig.InitializeRuntimeColors();
    public override List<Color> GetActiveColors() => _sandboxConfig.GetActiveColors();
    public override GameObjectListValue GetColorBlobs() => _sandboxConfig.ColorBlobs;
}

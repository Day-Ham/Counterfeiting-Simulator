using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SandboxCrayonAdapter", menuName = "Crayons/SandboxAdapter")]
public class SandBoxCrayonAdapter : GameModeCrayon
{
    [SerializeField] private SandboxConfigRuntimeAsset _runtimeAsset;

    public override void InitializeColors() => _runtimeAsset.Value.InitializeRuntimeColors();
    public override List<Color> GetActiveColors() => _runtimeAsset.Value.GetActiveColors();
    public override GameObjectListValue GetColorBlobs() => _runtimeAsset.Value.ColorBlobs;
}

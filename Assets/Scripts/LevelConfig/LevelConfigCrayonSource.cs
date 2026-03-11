using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfigCrayonSource : ICrayonSource
{
    private LevelConfigRuntimeAsset _runtimeAsset;

    public LevelConfigCrayonSource(LevelConfigRuntimeAsset runtimeAsset)
    {
        _runtimeAsset = runtimeAsset;
    }

    public void InitializeColors() => _runtimeAsset.Value.InitializeRuntimeWhiteColors();

    public List<Color> GetActiveColors() => _runtimeAsset.Value.GetActiveColors();

    public GameObjectListValue GetColorBlobs() => _runtimeAsset.Value.ColorBlobs;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCrayonAdapter", menuName = "Crayons/LevelAdapter")]
public class LevelCrayonAdapter : GameModeCrayon
{
    [SerializeField] private LevelConfigRuntimeAsset _runtimeAsset;

    public override void InitializeColors()
    {
        _runtimeAsset.Value.InitializeRuntimeWhiteColors();
    }

    public override List<Color> GetActiveColors()
    {
        // Use runtime white colors only if ColorPicker mode
        if (_runtimeAsset.Value.GameMode == LevelGameMode.ColorPicker)
        {
            return _runtimeAsset.Value.GetActiveColors(); // returns _runtimeWhiteColors
        }
        else
        {
            return _runtimeAsset.Value.ColorsToBeUsed?.Value;
        }
    }

    public override GameObjectListValue GetColorBlobs()
    {
        return _runtimeAsset.Value.ColorBlobs;
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSandBoxConfig", menuName = "Level/SandboxConfig")]
public class SandboxModeConfig : ScriptableObject
{
    [Header("Core Data")]
    public CanvasTemplateValue CanvasTemplate;
    public ColorDataValue ColorBackgroundDraw;

    [Tooltip("Editable colors for sandbox mode")]
    public ColorDataListValue ColorsToBeUsed;

    [Header("Universal Data")] 
    public GameObjectListValue ColorBlobs;
    
    private List<Color> _runtimeColors;
    
    public void InitializeRuntimeColors()
    {
        _runtimeColors = new List<Color>(ColorsToBeUsed?.Value ?? new List<Color>());
    }
    
    public List<Color> GetActiveColors()
    {
        if (_runtimeColors == null) InitializeRuntimeColors();

        return _runtimeColors;
    }
    
    public void SetColor(int index, Color newColor)
    {
        if (_runtimeColors == null) InitializeRuntimeColors();

        if (index < 0 || index >= _runtimeColors.Count) return;

        _runtimeColors[index] = newColor;
    }
}

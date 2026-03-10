using UnityEngine;
using DaeHanKim.ThisIsTotallyADollar.Drawing;

public class DrawingColorReceiver : MonoBehaviour
{
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    [SerializeField] private CanvasDrawController DrawController;
    [SerializeField] private CanvasLayerDrawController LayerController;
    [SerializeField] private LevelConfigRuntimeAsset LevelConfigRuntimeAsset;
    
    private LevelConfig _currentLevel;
    
    private void Awake()
    {
        _currentLevel = LevelConfigRuntimeAsset.Value;
    }


    private void OnEnable()
    {
        SelectColorEvent.OnColorSelected += OnColorSelected;
        SelectColorEvent.OnEraseSelected += OnEraseSelected;
    }

    private void OnDisable()
    {
        SelectColorEvent.OnColorSelected -= OnColorSelected;
        SelectColorEvent.OnEraseSelected -= OnEraseSelected;
    }

    private void OnColorSelected(int index)
    {
        var colors = _currentLevel.GetActiveColors();

        if (colors == null || index < 0 || index >= colors.Count) return;

        Color selectedColor = colors[index];

        LayerController.SetBrushColor(selectedColor);

        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Draw);
    }

    private void OnEraseSelected()
    {
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Erase);
    }
}

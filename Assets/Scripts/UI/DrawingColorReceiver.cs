using UnityEngine;
using DaeHanKim.ThisIsTotallyADollar.Drawing;

public class DrawingColorReceiver : MonoBehaviour
{
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    [SerializeField] private CanvasDrawController DrawController;
    [SerializeField] private CanvasLayerDrawController LayerController;

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

    private void OnColorSelected(Color color)
    {
        bool isColorPickerMode = DrawController.LevelConfigRuntime.Value.GameMode == ColorGameMode.ColorPicker;
        Color brushColor = isColorPickerMode ? Color.white : color;
        
        LayerController.SetBrushColor(brushColor);
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Draw);
    }

    private void OnEraseSelected()
    {
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Erase);
    }
}

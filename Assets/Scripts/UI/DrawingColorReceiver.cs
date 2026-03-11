using UnityEngine;
using DaeHanKim.ThisIsTotallyADollar.Drawing;

public class DrawingColorReceiver : MonoBehaviour
{
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    [SerializeField] private CanvasDrawController DrawController;
    [SerializeField] private CanvasLayerDrawController LayerController;
    [SerializeField] private ConfigRuntime RuntimeAsset;

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
        if (RuntimeAsset == null || !RuntimeAsset.HasValue) return;

        var colors = RuntimeAsset.GetActiveColors();

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

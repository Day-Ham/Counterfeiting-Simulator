using UnityEngine;
using DaeHanKim.ThisIsTotallyADollar.Drawing;

public class CanvasDrawingColorReceiver : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private IntEvent colorSelectedEvent;
    [SerializeField] private VoidEvent eraserSelectEvent;
    
    [SerializeField] private CanvasDrawController drawController;
    [SerializeField] private CanvasLayerDrawController layerController;
    [SerializeField] private ConfigRuntime runtimeAsset;

    private void OnEnable()
    {
        colorSelectedEvent.Register(OnColorSelected);
        eraserSelectEvent.Register(OnEraseSelected);
    }

    private void OnDisable()
    {
        colorSelectedEvent.Unregister(OnColorSelected);
        eraserSelectEvent.Unregister(OnEraseSelected);
    }

    private void OnColorSelected(int index)
    {
        if (runtimeAsset == null || !runtimeAsset.HasValue) return;

        var colors = runtimeAsset.GetActiveColors();

        if (colors == null || index < 0 || index >= colors.Count) return;

        Color selectedColor = colors[index];

        layerController.SetBrushColor(selectedColor);

        drawController.SetDrawMode(CanvasDrawController.DrawMode.Draw);
    }

    private void OnEraseSelected()
    {
        drawController.SetDrawMode(CanvasDrawController.DrawMode.Erase);
    }
}

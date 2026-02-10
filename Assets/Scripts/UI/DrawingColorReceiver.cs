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
        LayerController.SetBrushColor(color);
        
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Draw);
    }

    private void OnEraseSelected()
    {
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Erase);
    }
}

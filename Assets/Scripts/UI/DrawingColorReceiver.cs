using UnityEngine;
using DaeHanKim.ThisIsTotallyADollar.Drawing;

//This Script is being used on the Canvas Section
public class DrawingColorReceiver : MonoBehaviour
{
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    [SerializeField] private CanvasDrawController DrawController;
    [SerializeField] private CanvasLayerDrawController LayerController;

    private void OnEnable()
    {
        SelectColorEvent.OnColorSelected += OnColorSelected;
    }

    private void OnDisable()
    {
        SelectColorEvent.OnColorSelected -= OnColorSelected;
    }

    private void OnColorSelected(Color color)
    {
        LayerController.SetBrushColor(color);
        
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Draw);
    }

    public void SetEraser()
    {
        DrawController.SetDrawMode(CanvasDrawController.DrawMode.Erase);
    }
}

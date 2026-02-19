using System.Collections;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;
using UnityEngine.UI;
public class ScaleSlider : MonoBehaviour
{
    public Slider BrushScaleSlider;
    public CanvasDrawControllerValue BrushController;
    public GameObjectValue Cursor;

    public float ReferenceNumber = 64;

    private CanvasDrawController CanvasDrawController => BrushController.Value;

    public void SetSize()
    {
        if (CanvasDrawController == null || Cursor.Value == null)
            return;
        
        Transform cursorSize = Cursor.Value.transform;
        
        cursorSize.localScale = new Vector3(BrushScaleSlider.value, BrushScaleSlider.value, BrushScaleSlider.value);
        CanvasDrawController.SetBrushSize(BrushScaleSlider.value * ReferenceNumber);
    }
}

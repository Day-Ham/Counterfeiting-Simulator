using System.Collections;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;
using UnityEngine.UI;
public class ScaleSlider : MonoBehaviour
{
    public Slider BrushScaleSlider;
    public CanvasDrawController BrushController;
    public GameObjectValue Cursor;

    public float ReferenceNumber = 64;

    void Start()
    {
        BrushController.SetBrushSize(BrushScaleSlider.value * ReferenceNumber);
    }

    public void SetSize()
    {
        Transform cursorSize = Cursor.Value.transform;
        
        cursorSize.localScale = new Vector3(BrushScaleSlider.value, BrushScaleSlider.value, BrushScaleSlider.value);
        BrushController.SetBrushSize(BrushScaleSlider.value * ReferenceNumber);
    }
}

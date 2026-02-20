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
    
    private void Awake()
    {
        if (BrushScaleSlider != null)
        {
            BrushScaleSlider.onValueChanged.AddListener(SetSize);
        }
    }

    private void OnDestroy()
    {
        if (BrushScaleSlider != null)
        {
            BrushScaleSlider.onValueChanged.RemoveListener(SetSize);
        }
    }
    
    private void SetSize(float brushScaleSize)
    {
        if (CanvasDrawController == null || Cursor.Value == null)
        {
            return;
        }
        
        Transform cursorSize = Cursor.Value.transform;

        cursorSize.localScale = Vector3.one * brushScaleSize;
        CanvasDrawController.SetBrushSize(brushScaleSize * ReferenceNumber);
    }
}

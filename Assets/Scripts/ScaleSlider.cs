using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ScaleSlider : MonoBehaviour
{
    public Slider BrushScaleSlider;
    public CanvasDrawControllerValue BrushController;
    public GameObjectValue Cursor;
    
    public RectTransform SliderRect;
    public Canvas ParentCanvas;
    
    public float ReferenceNumber = 64;
    public float ScrollSensitivity;
    
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
    
    private void Update()
    {
        // Check if mouse is inside full slider area
        if (!RectTransformUtility.RectangleContainsScreenPoint(SliderRect, Input.mousePosition, ParentCanvas.worldCamera))
        {
            return;
        };
        
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newValue = BrushScaleSlider.value + scroll * ScrollSensitivity;
            BrushScaleSlider.value = Mathf.Clamp(newValue, BrushScaleSlider.minValue, BrushScaleSlider.maxValue);
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

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [Header("RGB Sliders In-Order")]
    [SerializeField] private List<Slider> RGBSliders = new();
    [SerializeField] private List<TMP_Text> RGBNumberTexts = new();
    [SerializeField] private List<RectTransform> SliderRects = new();
    [SerializeField] private Canvas ParentCanvas;
    
    [Header("Scroll Settings")]
    [Range(0.01f, 1f)]
    [SerializeField] private float ScrollSensitivity;

    [Header("Color Preview")]
    [SerializeField] private Image colorPreview;

    [Header("Events")]
    [SerializeField] private SelectedColorEvent _selectedColorEvent;
    [SerializeField] private SelectBrushColorEvent _selectBrushColorEvent;
    [SerializeField] private OpenColorPickerEvent _openColorPickerEvent;
    
    private Slider hoveredSlider;
    
    private void OnEnable()
    {
        _openColorPickerEvent.OnColorPickerOpened += SetColor;
    }

    private void OnDisable()
    {
        _openColorPickerEvent.OnColorPickerOpened -= SetColor;
    }

    private void Awake()
    {
        for (int i = 0; i < RGBSliders.Count; i++)
        {
            int index = i; // local copy for closure
            RGBSliders[i].onValueChanged.AddListener(_ => UpdateColorPreview());
        }

        UpdateColorPreview();
    }
    
    private void Update()
    {
        for (int i = 0; i < RGBSliders.Count; i++)
        {
            if (i >= SliderRects.Count) continue;

            // Only adjust if mouse is inside the slider rect
            if (!RectTransformUtility.RectangleContainsScreenPoint(SliderRects[i], Input.mousePosition, ParentCanvas.worldCamera)) continue;

            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) <= 0.01f) continue;

            float newColorValue = RGBSliders[i].value + scroll * ScrollSensitivity;
            newColorValue = Mathf.Clamp(newColorValue, RGBSliders[i].minValue, RGBSliders[i].maxValue);
            
            // Snap to nearest 1/255
            newColorValue = Mathf.Round(newColorValue * 255f) / 255f;
            
            RGBSliders[i].SetValueWithoutNotify(newColorValue);
            
            RGBSliders[i].onValueChanged.Invoke(newColorValue);
        }
    }

    private void UpdateColorPreview()
    {
        if (RGBSliders.Count < 3)
        {
            return;
        }

        Color newColor = new Color(
            RGBSliders[0].value,
            RGBSliders[1].value,
            RGBSliders[2].value
        );

        // Update preview
        if (colorPreview)
        {
            colorPreview.color = newColor;
        }

        // Update integer texts (0-255)
        for (int i = 0; i < RGBNumberTexts.Count && i < RGBSliders.Count; i++)
        {
            if (RGBNumberTexts[i])
            {
                RGBNumberTexts[i].text = Mathf.RoundToInt(RGBSliders[i].value * 255).ToString();
            }
        }

        RaiseColorPicked(newColor);
    }
    
    private void RaiseColorPicked(Color newColor)
    {
        int index = _selectBrushColorEvent.CurrentSelectedIndex;

        if (index < 0) return;

        _selectedColorEvent.Raise(index, newColor);
        
        _selectBrushColorEvent.Raise(index);
    }
    
    private void SetColor(Color newColor)
    {
        if (RGBSliders.Count < 3) return;

        RGBSliders[0].SetValueWithoutNotify(newColor.r);
        RGBSliders[1].SetValueWithoutNotify(newColor.g);
        RGBSliders[2].SetValueWithoutNotify(newColor.b);

        UpdateColorPreview();
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [Header("RGB Sliders In-Order")]
    [SerializeField] private List<Slider> RGBSliders = new();
    [SerializeField] private List<TMP_InputField> RGBInputFields = new();
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
    
    private const int RGBMAX = 255;
    
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
        SetupSliders();
        SetupInputFields();
        UpdateColorPreview();
    }
    
    private void SetupSliders()
    {
        foreach (var rgbSlider in RGBSliders)
        {
            rgbSlider.onValueChanged.AddListener(_ => UpdateColorPreview());
        }
    }
    
    private void SetupInputFields()
    {
        for (int i = 0; i < RGBInputFields.Count; i++)
        {
            int index = i;

            RGBInputFields[i].onEndEdit.AddListener(input =>
            {
                if (!int.TryParse(input, out int value)) return;

                value = Mathf.Clamp(value, 0, RGBMAX);

                SetSliderValue(index, value / (float)RGBMAX);
                UpdateColorPreview();
            });
        }
    }
    
    private void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) <= 0.01f) return;

        for (int i = 0; i < RGBSliders.Count; i++)
        {
            if (!IsMouseOverSlider(i)) continue;

            float newValue = RGBSliders[i].value + scroll * ScrollSensitivity;
            newValue = Mathf.Clamp(newValue, RGBSliders[i].minValue, RGBSliders[i].maxValue);

            SetSliderValue(i, newValue);
        }
    }
    
    private bool IsMouseOverSlider(int index)
    {
        if (index >= SliderRects.Count) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(
            SliderRects[index],
            Input.mousePosition,
            ParentCanvas.worldCamera
        );
    }
    
    private void SetSliderValue(int index, float value)
    {
        RGBSliders[index].SetValueWithoutNotify(value);
        RGBSliders[index].onValueChanged.Invoke(value);
    }

    private void UpdateColorPreview()
    {
        if (RGBSliders.Count < 3) return;

        Color newColor = GetCurrentColor();

        if (colorPreview)
        {
            colorPreview.color = newColor;
        }

        UpdateInputFields();
        RaiseColorPicked(newColor);
    }
    
    private Color GetCurrentColor()
    {
        return new Color(
            RGBSliders[0].value,
            RGBSliders[1].value,
            RGBSliders[2].value
        );
    }
    
    private void UpdateInputFields()
    {
        for (int i = 0; i < RGBInputFields.Count && i < RGBSliders.Count; i++)
        {
            TMP_InputField rgbInputField = RGBInputFields[i];
            if (!rgbInputField || rgbInputField.isFocused) continue;

            int value = Mathf.RoundToInt(RGBSliders[i].value * RGBMAX);
            rgbInputField.SetTextWithoutNotify(value.ToString());
        }
    }
    
    private void RaiseColorPicked(Color newColor)
    {
        int selectedIndex = _selectBrushColorEvent.CurrentSelectedIndex;

        if (selectedIndex < 0) return;

        _selectedColorEvent.Raise(selectedIndex, newColor);
        _selectBrushColorEvent.Raise(selectedIndex);
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
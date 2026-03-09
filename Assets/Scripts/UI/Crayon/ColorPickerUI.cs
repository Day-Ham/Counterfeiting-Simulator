using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [Header("RGB Sliders In-Order")]
    [SerializeField] private List<RGBChannel> RGBChannels = new();
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
    
    private const int RGB_MAX = 255;
    
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
        SetupRGBChannels();
        UpdateColorPreview();
    }
    
    private void SetupRGBChannels()
    {
        for (int i = 0; i < RGBChannels.Count; i++)
        {
            int index = i;
            var rgbChannel = RGBChannels[i];

            rgbChannel.Slider.onValueChanged.AddListener(_ => UpdateColorPreview());

            rgbChannel.InputField.onEndEdit.AddListener(input =>
            {
                if (!int.TryParse(input, out int value)) return;

                value = Mathf.Clamp(value, 0, 255);

                SetSliderValue(index, value / 255f);
                UpdateColorPreview();
            });
        }
    }
    
    private void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) <= 0.01f) return;

        for (int i = 0; i < RGBChannels.Count; i++)
        {
            if (!IsMouseOverChannel(i)) continue;

            var slider = RGBChannels[i].Slider;

            float value = slider.value + scroll * ScrollSensitivity;
            value = Mathf.Clamp(value, slider.minValue, slider.maxValue);

            SetSliderValue(i, value);
        }
    }
    
    private bool IsMouseOverChannel(int index)
    {
        var rect = RGBChannels[index].Rect;

        return RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            Input.mousePosition,
            ParentCanvas.worldCamera
        );
    }
    
    private void SetSliderValue(int index, float value)
    {
        var rgbSliders = RGBChannels[index].Slider;

        if (!rgbSliders) return;

        rgbSliders.SetValueWithoutNotify(value);
        rgbSliders.onValueChanged.Invoke(value);
    }

    private void UpdateColorPreview()
    {
        if (RGBChannels.Count < 3) return;

        Color newColor = GetCurrentColor();

        if (colorPreview != null)
        {
            colorPreview.color = newColor;
        }

        UpdateInputFields();
        RaiseColorPicked(newColor);
    }
    
    private Color GetCurrentColor()
    {
        return new Color(
            RGBChannels[0].Slider.value,
            RGBChannels[1].Slider.value,
            RGBChannels[2].Slider.value
        );
    }
    
    private void UpdateInputFields()
    {
        foreach (var channel in RGBChannels)
        {
            if (channel.InputField == null || channel.InputField.isFocused) continue;

            int value = Mathf.RoundToInt(channel.Slider.value * RGB_MAX);
            channel.InputField.SetTextWithoutNotify(value.ToString());
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
        if (RGBChannels.Count < 3) return;

        SetSliderValue(0, newColor.r);
        SetSliderValue(1, newColor.g);
        SetSliderValue(2, newColor.b);

        UpdateColorPreview();
    }
}
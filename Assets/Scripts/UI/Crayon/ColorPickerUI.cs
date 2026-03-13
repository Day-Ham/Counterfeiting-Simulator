using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [Header("RGB Sliders In-Order")]
    [SerializeField] private List<RGBChannel> RGBChannels = new();
    [SerializeField] private Canvas ParentCanvas;
    [SerializeField] private ConfigRuntime RuntimeAsset;
    
    [Header("Scroll Settings")]
    [Range(0.01f, 1f)]
    [SerializeField] private float ScrollSensitivity;

    [Header("Color Preview")]
    [SerializeField] private Image colorPreview;

    [Header("Events")]
    [SerializeField] private SelectedColorEvent _selectedColorEvent;
    [SerializeField] private SelectBrushColorEvent _selectBrushColorEvent;
    [SerializeField] private OpenColorPickerEvent _openColorPickerEvent;

    private const int MIN_RGB = 0;
    private const int MAX_RGB = 255;
    
    // Cached to avoid recomputing every frame
    private bool _isColorPickerMode;
    private int _cachedSelectedIndex = -1;
    
    private void OnEnable()
    {
        _openColorPickerEvent.OnColorPickerOpened += SetColor;
        if (RuntimeAsset != null)
            RuntimeAsset.OnValueChanged += OnRuntimeChanged;
    }

    private void OnDisable()
    {
        _openColorPickerEvent.OnColorPickerOpened -= SetColor;
        if (RuntimeAsset != null)
            RuntimeAsset.OnValueChanged -= OnRuntimeChanged;
    }

    private void Awake()
    {
        SetupRGBChannels();
        UpdateColorPreview(GetCurrentColor());
    }
    
    private void OnRuntimeChanged()
    {
        UpdateColorPreview(GetCurrentColor());
    }
    
    private void SetupRGBChannels()
    {
        for (int i = 0; i < RGBChannels.Count; i++)
        {
            int index = i;
            var channel = RGBChannels[i];
            
            channel.Slider.onValueChanged.AddListener(_ => UpdateColorPreview(GetCurrentColor()));
            
            channel.InputField.onEndEdit.AddListener(input =>
            {
                if (!int.TryParse(input, out int value)) return;

                value = Mathf.Clamp(value, MIN_RGB, MAX_RGB);
                
                channel.Slider.SetValueWithoutNotify(value);
                
                UpdateColorPreview(GetCurrentColor());
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
            float value = Mathf.Clamp(slider.value + scroll * ScrollSensitivity, slider.minValue, slider.maxValue);
            int rounded = Mathf.RoundToInt(value);
            
            slider.value = rounded;
            
            break;
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
        if (index < 0 || index >= RGBChannels.Count) return;
        
        RGBChannels[index].Slider.SetValueWithoutNotify(value);
        
        int rounded = Mathf.RoundToInt(value);
        RGBChannels[index].InputField.SetTextWithoutNotify(rounded.ToString());
    }

    private void UpdateColorPreview(Color color)
    {
        _cachedSelectedIndex = _selectBrushColorEvent.CurrentSelectedIndex;

        if (_cachedSelectedIndex >= 0 && RuntimeAsset != null && RuntimeAsset.HasValue)
        {
            var activeColors = RuntimeAsset.GetActiveColors();
            
            if (activeColors != null)
            {
                color = ColorMatchUtils.SnapPerChannelClosest(color, activeColors, 10); // use default tolerance or provide one
            }

            // Only update sliders/runtime if the snapped color actually changed
            if (RuntimeAsset is LevelConfigRuntimeAsset levelRuntime)
            {
                levelRuntime.Value.SetWhiteColor(_cachedSelectedIndex, color);
            }
            else if (RuntimeAsset is SandboxConfigRuntimeAsset sandboxRuntime)
            {
                sandboxRuntime.Value.SetColor(_cachedSelectedIndex, color);
            }
            
            SetSliderValue(0, color.r * 255);
            SetSliderValue(1, color.g * 255);
            SetSliderValue(2, color.b * 255);
        }

        if (colorPreview)
        {
            colorPreview.color = color;
        }
        
        UpdateInputFields();

        if (_cachedSelectedIndex < 0) return;
        
        _selectedColorEvent.Raise(_cachedSelectedIndex, color);
        _selectBrushColorEvent.Raise(_cachedSelectedIndex);
    }
    
    private Color GetCurrentColor()
    {
        return ColorUtils.FromRGB(
            Mathf.RoundToInt(RGBChannels[0].Slider.value),
            Mathf.RoundToInt(RGBChannels[1].Slider.value),
            Mathf.RoundToInt(RGBChannels[2].Slider.value)
        );
    }
    
    private void UpdateInputFields()
    {
        foreach (var channel in RGBChannels)
        {
            if (!channel.InputField || channel.InputField.isFocused) continue;

            int value = Mathf.RoundToInt(channel.Slider.value);
            string newText = value.ToString();
            
            if (channel.InputField.text != newText)
            {
                channel.InputField.SetTextWithoutNotify(newText);
            }
        }
    }
    
    private void SetColor(Color newColor)
    {
        SetSliderValue(0, newColor.r * 255f);
        SetSliderValue(1, newColor.g * 255f);
        SetSliderValue(2, newColor.b * 255f);

        UpdateColorPreview(GetCurrentColor());
    }
}
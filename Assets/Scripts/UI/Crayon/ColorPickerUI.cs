using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [Header("RGB Sliders In-Order")]
    [SerializeField] private List<RGBChannel> RGBChannels = new();
    [SerializeField] private Canvas ParentCanvas;
    [SerializeField] private LevelConfigRuntimeAsset LevelRuntime;
    
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
        CacheGameModeState();
    }

    private void OnDisable()
    {
        _openColorPickerEvent.OnColorPickerOpened -= SetColor;
    }

    private void Awake()
    {
        SetupRGBChannels();
        UpdateColorPreview(GetCurrentColor());
    }
    
    // Cache whether we're in ColorPicker mode so we don't re-check it every preview update
    private void CacheGameModeState()
    {
        _isColorPickerMode = LevelRuntimeExists() && LevelRuntime.Value.GameMode == LevelGameMode.ColorPicker;
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

        if (_cachedSelectedIndex >= 0 && _isColorPickerMode && LevelRuntimeExists())
        {
            Color snapped = ColorMatchUtils.SnapPerChannelClosest(color, LevelRuntime.Value.ColorsToBeUsed.Value, LevelRuntime.Value.ColorMatcherData.Tolerance);

            // Only update sliders/runtime if the snapped color actually changed
            if (snapped != color)
            {
                SetSliderValue(0, snapped.r * 255);
                SetSliderValue(1, snapped.g * 255);
                SetSliderValue(2, snapped.b * 255);

                LevelRuntime.Value.SetWhiteColor(_cachedSelectedIndex, snapped);

                color = snapped;
            }
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
        if (RGBChannels.Count < 3) return;

        // Snap new color to ColorsToBeUsed
        if (LevelRuntimeExists() && LevelRuntime.Value.ColorMatcherData != null)
        {
            newColor = ColorMatchUtils.SnapPerChannelClosest(newColor, LevelRuntime.Value.ColorsToBeUsed.Value, LevelRuntime.Value.ColorMatcherData.Tolerance);
        }
        
        SetSliderValue(0, Mathf.RoundToInt(newColor.r * 255f));
        SetSliderValue(1, Mathf.RoundToInt(newColor.g * 255f));
        SetSliderValue(2, Mathf.RoundToInt(newColor.b * 255f));

        UpdateColorPreview(GetCurrentColor());
    }
    
    private bool LevelRuntimeExists()
    {
        return LevelRuntime && LevelRuntime.Value;
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private SelectedColorEvent selectedColorEvent;
    [SerializeField] private SelectBrushColorEvent selectBrushColorEvent;
    [SerializeField] private OpenColorPickerEvent openColorPickerEvent;
    
    [Header("RGB Sliders In-Order")]
    [SerializeField] private List<RGBChannel> rgbChannels = new();
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private ConfigRuntime runtimeAsset;
    
    [Header("Scroll Settings")]
    [Range(0.01f, 1f)]
    [SerializeField] private float scrollSensitivity;

    [Header("Color Preview")]
    [SerializeField] private Image colorPreview;

    private const int MinRGB = 0;
    private const int MaxRGB = 255;
    private const int SnapTolerance = 10;
    
    // Cached to avoid recomputing every frame
    private bool _isColorPickerMode;
    private int _cachedSelectedIndex = -1;
    
    private void OnEnable()
    {
        openColorPickerEvent.OnColorPickerOpened += SetColor;
        runtimeAsset.OnValueChanged += OnRuntimeChanged;
    }

    private void OnDisable()
    {
        openColorPickerEvent.OnColorPickerOpened -= SetColor;
        runtimeAsset.OnValueChanged -= OnRuntimeChanged;
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
        for (int i = 0; i < rgbChannels.Count; i++)
        {
            int index = i;
            var channel = rgbChannels[i];
            
            channel.slider.onValueChanged.AddListener(_ => UpdateColorPreview(GetCurrentColor()));
            
            channel.inputField.onEndEdit.AddListener(input =>
            {
                if (!int.TryParse(input, out int value)) return;

                value = Mathf.Clamp(value, MinRGB, MaxRGB);
                
                channel.slider.SetValueWithoutNotify(value);
                
                UpdateColorPreview(GetCurrentColor());
            });
        }
    }
    
    private void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) <= 0.01f) return;

        for (int i = 0; i < rgbChannels.Count; i++)
        {
            if (!IsMouseOverChannel(i)) continue;

            var slider = rgbChannels[i].slider;
            float value = Mathf.Clamp(slider.value + scroll * scrollSensitivity, slider.minValue, slider.maxValue);
            int rounded = Mathf.RoundToInt(value);
            
            slider.value = rounded;
            
            break;
        }
    }
    
    private bool IsMouseOverChannel(int index)
    {
        var rect = rgbChannels[index].rectTransform;

        return RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            Input.mousePosition,
            parentCanvas.worldCamera
        );
    }
    
    private void SetSliderValue(int index, float value)
    {
        if (index < 0 || index >= rgbChannels.Count) return;
        
        rgbChannels[index].slider.SetValueWithoutNotify(value);
        
        int rounded = Mathf.RoundToInt(value);
        rgbChannels[index].inputField.SetTextWithoutNotify(rounded.ToString());
    }

    private void UpdateColorPreview(Color color)
    {
        _cachedSelectedIndex = selectBrushColorEvent.CurrentSelectedIndex;

        if (_cachedSelectedIndex >= 0 && runtimeAsset != null && runtimeAsset.HasValue)
        {
            var activeColors = runtimeAsset.GetActiveColors();

            if (runtimeAsset.UseSnapping && activeColors != null)
            {
                color = ColorMatchUtils.SnapPerChannelClosest(color, activeColors, SnapTolerance);
            }

            // Only update sliders/runtime if the snapped color actually changed
            if (runtimeAsset is LevelConfigRuntimeAsset levelRuntime)
            {
                levelRuntime.Value.SetWhiteColor(_cachedSelectedIndex, color);
            }
            else if (runtimeAsset is SandboxConfigRuntimeAsset sandboxRuntime)
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
        
        selectedColorEvent.Raise(_cachedSelectedIndex, color);
        selectBrushColorEvent.Raise(_cachedSelectedIndex);
    }
    
    private Color GetCurrentColor()
    {
        return ColorUtils.FromRGB(
            Mathf.RoundToInt(rgbChannels[0].slider.value),
            Mathf.RoundToInt(rgbChannels[1].slider.value),
            Mathf.RoundToInt(rgbChannels[2].slider.value)
        );
    }
    
    private void UpdateInputFields()
    {
        foreach (var channel in rgbChannels)
        {
            if (!channel.inputField || channel.inputField.isFocused) continue;

            int value = Mathf.RoundToInt(channel.slider.value);
            string newText = value.ToString();
            
            if (channel.inputField.text != newText)
            {
                channel.inputField.SetTextWithoutNotify(newText);
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
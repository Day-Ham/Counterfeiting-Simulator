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
            
            slider.SetValueWithoutNotify(Mathf.RoundToInt(value));
            
            UpdateColorPreview(GetCurrentColor());
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
        
        RGBChannels[index].InputField.SetTextWithoutNotify(Mathf.RoundToInt(value).ToString());
    }

    private void UpdateColorPreview(Color color)
    {
        // Snap color if in ColorPicker mode
        int selectedIndex = _selectBrushColorEvent.CurrentSelectedIndex;
        if (selectedIndex >= 0 && LevelRuntimeExists())
        {
            if (LevelRuntime.Value.LevelGameMode == LevelGameMode.ColorPicker)
            {
                // Snap color
                Color snapped = LevelRuntime.Value.ColorMatcher.SnapPerChannel(color, LevelRuntime.Value.ColorsToBeUsed.Value);

                // **Set sliders without triggering events**
                SetSliderValue(0, snapped.r * 255);
                SetSliderValue(1, snapped.g * 255);
                SetSliderValue(2, snapped.b * 255);

                // Update runtime color
                LevelRuntime.Value.SetWhiteColor(selectedIndex, snapped);

                color = snapped;
            }
        }

        // Update preview image
        if (colorPreview)
        {
            colorPreview.color = color;
        }

        // Update InputFields
        UpdateInputFields();

        // Raise events
        if (selectedIndex < 0) return;
        
        _selectedColorEvent.Raise(selectedIndex, color);
        _selectBrushColorEvent.Raise(selectedIndex);
    }
    
    private Color GetCurrentColor()
    {
        return new Color(
            RGBChannels[0].Slider.value / 255f,
            RGBChannels[1].Slider.value / 255f,
            RGBChannels[2].Slider.value / 255f
        );
    }
    
    private void UpdateInputFields()
    {
        foreach (var channel in RGBChannels)
        {
            if (!channel.InputField || channel.InputField.isFocused) continue;

            int value = Mathf.RoundToInt(channel.Slider.value);
            channel.InputField.SetTextWithoutNotify(value.ToString());
        }
    }
    
    private void SetColor(Color newColor)
    {
        if (RGBChannels.Count < 3) return;

        // Snap new color to ColorsToBeUsed
        if (LevelRuntimeExists() && LevelRuntime.Value.ColorMatcher != null)
        {
            newColor = LevelRuntime.Value.ColorMatcher.SnapPerChannel(newColor, LevelRuntime.Value.ColorsToBeUsed.Value);
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
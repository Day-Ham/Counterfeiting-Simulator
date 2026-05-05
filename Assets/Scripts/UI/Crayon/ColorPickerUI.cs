using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ColorPickerUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private SelectedColorEvent selectedColorEvent;
    [SerializeField] private IntEvent selectBrushColorEvent;
    [SerializeField] private ColorEvent colorEvent;
    
    [Header("RGB Sliders In-Order")]
    [SerializeField] private List<RGBChannel> rgbChannels = new();
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private ConfigRuntime runtimeAsset;
    
    [Header("Scroll Settings")]
    [Range(0.01f, 1f)]
    [SerializeField] private float scrollSensitivity;
    [SerializeField] private float sliderLerpDuration = 0.2f;

    [Header("Color Preview")]
    [SerializeField] private Image colorPreview;
    
    private const int SnapTolerance = 10;
    
    // Cached to avoid recomputing every frame
    private int _cachedSelectedIndex = -1;
    
    private void OnEnable()
    {
        selectBrushColorEvent.Register(OnColorSelected);
        colorEvent.Register(SetColor);
        runtimeAsset.OnValueChanged += RefreshPreview;
    }

    private void OnDisable()
    {
        selectBrushColorEvent.Unregister(OnColorSelected);
        colorEvent.Unregister(SetColor);
        runtimeAsset.OnValueChanged -= RefreshPreview;
    }

    private void Awake()
    {
        SetupChannels();
        RefreshPreview();
    }

    private void Update()
    {
        HandleScrollInput();
    }

    private void SetupChannels()
    {
        for (int i = 0; i < rgbChannels.Count; i++)
        {
            SetupSlider(rgbChannels[i]);
            SetupInputField(rgbChannels[i]);
        }
    }

    private void SetupSlider(RGBChannel channel)
    {
        channel.slider.onValueChanged.AddListener(_ => OnSliderChanged());
    }

    private void SetupInputField(RGBChannel rgbChannel)
    {
        InputFieldUtility.SetupRGBInput(rgbChannel.inputField, rgbChannel, OnRGBInputChanged);
    }
    
    private void OnRGBInputChanged(RGBChannel channel, int value)
    {
        channel.slider.SetValueWithoutNotify(value);
        OnSliderChanged();
    }

    private void HandleScrollInput()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) <= 0.01f) return;

        int index = GetHoveredChannel();
        if (index == -1) return;

        AdjustSlider(index, scroll);
    }
    
    private int GetHoveredChannel()
    {
        for (int i = 0; i < rgbChannels.Count; i++)
        {
            if (IsMouseOver(rgbChannels[i])) return i;
        }
        return -1;
    }

    private bool IsMouseOver(RGBChannel channel)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            channel.rectTransform,
            Input.mousePosition,
            parentCanvas.worldCamera
        );
    }

    private void AdjustSlider(int index, float scroll)
    {
        var slider = rgbChannels[index].slider;

        float value = slider.value + scroll * scrollSensitivity;
        value = Mathf.Clamp(value, slider.minValue, slider.maxValue);

        slider.value = Mathf.RoundToInt(value);
    }

    private void OnSliderChanged()
    {
        ApplyColor(GetCurrentColor());
    }
    
    private void OnColorSelected(int index)
    {
        _cachedSelectedIndex = index;
    }

    private void ApplyColor(Color color)
    {
        if (_cachedSelectedIndex < 0) return;

        color = ApplySnapping(color);
        ApplyToRuntime(color);
        SyncUI(color);
        RaiseEvents(color);
    }

    private Color ApplySnapping(Color color)
    {
        if (!CanSnap()) return color;

        var activeColors = runtimeAsset.GetActiveColors();
        return ColorMatchUtils.SnapPerChannelClosest(color, activeColors, SnapTolerance);
    }

    private bool CanSnap()
    {
        return runtimeAsset &&
               runtimeAsset.HasValue &&
               runtimeAsset.UseSnapping;
    }

    private void ApplyToRuntime(Color color)
    {
        if (_cachedSelectedIndex < 0 || !runtimeAsset) return;

        if (runtimeAsset is MainGameConfigRuntimeAsset level)
        {
            level.Value.SetWhiteColor(_cachedSelectedIndex, color);
        }
        else if (runtimeAsset is SandboxConfigRuntimeAsset sandbox)
        {
            sandbox.Value.SetColor(_cachedSelectedIndex, color);
        }
    }

    private void SyncUI(Color color)
    {
        SetSliderValues(color);
        UpdateInputFields();
        UpdatePreview(color);
    }

    private void SetSliderValues(Color color)
    {
        SetSlider(0, color.r * 255);
        SetSlider(1, color.g * 255);
        SetSlider(2, color.b * 255);
    }

    private void SetSlider(int index, float value)
    {
        if (index < 0 || index >= rgbChannels.Count) return;

        var channel = rgbChannels[index];

        channel.slider.DOKill();
        DOTween.To(
            () => channel.slider.value,
            x => {
                channel.slider.SetValueWithoutNotify(x);
                channel.inputField.SetTextWithoutNotify(Mathf.RoundToInt(x).ToString());
            },
            value,
            sliderLerpDuration
        );
    }

    private void UpdateInputFields()
    {
        foreach (var rgbChannel in rgbChannels)
        {
            if (!rgbChannel.inputField || rgbChannel.inputField.isFocused) continue;

            int value = Mathf.RoundToInt(rgbChannel.slider.value);
            rgbChannel.inputField.SetTextWithoutNotify(value.ToString());
        }
    }

    private void UpdatePreview(Color color)
    {
        if (colorPreview)
        {
            colorPreview.color = color;
        }
    }

    private void RaiseEvents(Color color)
    {
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

    private void RefreshPreview()
    {
        UpdatePreview(GetCurrentColor());
    }

    private void SetColor(Color color)
    {
        SetSliderValues(color);
        UpdatePreview(color);
    }
}
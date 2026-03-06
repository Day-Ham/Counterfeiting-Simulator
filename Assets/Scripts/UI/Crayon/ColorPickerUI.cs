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

    [SerializeField] private Image colorPreview;

    [Header("Events")]
    [SerializeField] private ColorPickedEvent _colorPickedEvent;
    [SerializeField] private SelectBrushColorEvent _selectBrushColorEvent;

    private void Awake()
    {
        for (int i = 0; i < RGBSliders.Count; i++)
        {
            int index = i; // local copy for closure
            RGBSliders[i].onValueChanged.AddListener(_ => UpdateColorPreview());
        }

        UpdateColorPreview();
    }

    private void UpdateColorPreview()
    {
        if (RGBSliders.Count < 3)
        {
            Debug.LogWarning("RGBColorPicker: Need exactly 3 sliders in the list!");
            return;
        }

        Color newColor = new Color(
            RGBSliders[0].value,
            RGBSliders[1].value,
            RGBSliders[2].value
        );

        // Update preview
        if (colorPreview != null)
        {
            colorPreview.color = newColor;
        }

        // Update integer texts (0-255)
        for (int i = 0; i < RGBNumberTexts.Count && i < RGBSliders.Count; i++)
        {
            if (RGBNumberTexts[i] != null)
            {
                RGBNumberTexts[i].text = Mathf.RoundToInt(RGBSliders[i].value * 255).ToString();
            }
        }

        RaiseColorPicked(newColor);
    }
    
    private void RaiseColorPicked(Color newColor)
    {
        if (_colorPickedEvent == null || _selectBrushColorEvent == null) return;

        int index = _selectBrushColorEvent.CurrentSelectedIndex;

        if (index < 0) return;

        _colorPickedEvent.Raise(index, newColor);
    }
}

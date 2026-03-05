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
    public UnityEvent<Color> OnColorChanged;

    private void Awake()
    {
        for (int i = 0; i < RGBSliders.Count; i++)
        {
            int index = i; // local copy for closure
            RGBSliders[i].onValueChanged.AddListener(_ => UpdateColorPreview());
        }

        UpdateColorPreview();
    }

    public void UpdateColorPreview()
    {
        if (RGBSliders.Count < 3)
        {
            Debug.LogWarning("RGBColorPicker: Need exactly 3 sliders in the list!");
            return;
        }

        Color newColor = new Color(RGBSliders[0].value, RGBSliders[1].value, RGBSliders[2].value);
        
        if (colorPreview != null)
        {
            colorPreview.color = newColor;
        }
        
        for (int i = 0; i < RGBNumberTexts.Count && i < RGBSliders.Count; i++)
        {
            if (RGBNumberTexts[i] != null)
            {
                RGBNumberTexts[i].text = Mathf.RoundToInt(RGBSliders[i].value * 255).ToString();
            }
        }

        OnColorChanged?.Invoke(newColor);
    }

    // Optional: programmatically set color
    public void SetColor(Color color)
    {
        if (RGBSliders.Count >= 3)
        {
            RGBSliders[0].value = color.r;
            RGBSliders[1].value = color.g;
            RGBSliders[2].value = color.b;
        }

        UpdateColorPreview();
    }

    public Color GetColor()
    {
        if (RGBSliders.Count >= 3)
        {
            return new Color(RGBSliders[0].value, RGBSliders[1].value, RGBSliders[2].value);
        }

        return Color.white;
    }

    public (int r, int g, int b) GetColorInt()
    {
        if (RGBSliders.Count >= 3)
            return (
                Mathf.RoundToInt(RGBSliders[0].value * 255),
                Mathf.RoundToInt(RGBSliders[1].value * 255),
                Mathf.RoundToInt(RGBSliders[2].value * 255)
            );

        return (255, 255, 255);
    }
}

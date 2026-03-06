using UnityEngine;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private ResizeTweenScriptableObject ResizeTweenScriptableObject;
    [SerializeField] private Button Button;
    [SerializeField] private Image ColorPreview;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    [SerializeField] private SetColorBlobLook SetColorBlobLook;
    
    [Header("Shadow Color")]
    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnSelectedColor;
    
    private Color color;
    private int colorIndex;
    
    private ColorPickerUI colorPickerUI;
    
    private void OnEnable()
    {
        SelectColorEvent.OnColorSelected += HandleColorSelected;
        SelectColorEvent.OnEraseSelected += HandleEraseSelected;
    }

    private void OnDisable()
    {
        SelectColorEvent.OnColorSelected -= HandleColorSelected;
        SelectColorEvent.OnEraseSelected -= HandleEraseSelected;
        
        if (colorPickerUI != null)
        {
            colorPickerUI.OnColorChanged.RemoveListener(UpdateColorFromPicker);
        };
    }
    
    private void HandleColorSelected(int selectedColorIndex)
    {
        if (selectedColorIndex == colorIndex)
        {
            ExpandSize();
            SetColorBlobLook.SetShadowColor(SelectedColor);
        }
        else
        {
            CollapseSize();
            SetColorBlobLook.SetShadowColor(UnSelectedColor);
        }
    }
    
    private void HandleEraseSelected()
    {
        CollapseSize();
        SetColorBlobLook.SetShadowColor(UnSelectedColor);
    }
        
    private void Awake()
    {
        CollapseSize();
        
        SetColorBlobLook.SetShadowColor(UnSelectedColor);
        
        Button.onClick.AddListener(OnClick);
    }

    public void Setup(Color newColor, int index, ColorPickerUI pickerUI)
    {
        color = newColor;
        colorIndex = index;
        ColorPreview.color = color;

        if (pickerUI == null) return;
        
        colorPickerUI = pickerUI;
            
        colorPickerUI.OnColorChanged.AddListener(UpdateColorFromPicker);
    }
    
    private void UpdateColorFromPicker(Color newColor)
    {
        if (SelectColorEvent.CurrentSelectedIndex != colorIndex) return;
        
        color = newColor;
        ColorPreview.color = color;
    }
    
    private void OnClick()
    {
        SelectColorEvent.Raise(colorIndex);
    }

    private void ExpandSize()
    {
        ResizeTweenScriptableObject.Expand(this.gameObject);
    }

    private void CollapseSize()
    {
        ResizeTweenScriptableObject.Collapse(this.gameObject);
    }
}

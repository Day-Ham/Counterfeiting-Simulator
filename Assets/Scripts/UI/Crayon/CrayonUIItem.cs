using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private ResizeTweenScriptableObject ResizeTweenScriptableObject;
    [SerializeField] private SetColorBlobLook SetColorBlobLook;
    [SerializeField] private LevelConfigRuntimeAsset LevelRuntime;
    [SerializeField] private GameObjectValue RGBSliderUI;
    
    [Header("UI")]
    [SerializeField] private Button Button;
    [SerializeField] private Image ColorPreview;
    
    [Header("Events")]
    [SerializeField] private OpenColorPickerEvent _openColorPickerEvent;
    [SerializeField] private SelectedColorEvent _selectedColorEvent;
    [SerializeField] private SelectBrushColorEvent _selectBrushColorEvent;
    
    [Header("Shadow Color")]
    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnSelectedColor;
    
    private Color color;
    private int colorIndex;
    
    private void OnEnable()
    {
        _selectBrushColorEvent.OnColorSelected += HandleBrushColorSelected;
        _selectBrushColorEvent.OnEraseSelected += HandleEraseSelected;
        _selectedColorEvent.OnColorPicked += HandleSelectedColor;
    }

    private void OnDisable()
    {
        _selectBrushColorEvent.OnColorSelected -= HandleBrushColorSelected;
        _selectBrushColorEvent.OnEraseSelected -= HandleEraseSelected;
        _selectedColorEvent.OnColorPicked -= HandleSelectedColor;
    }
    
    private void HandleBrushColorSelected(int selectedColorIndex)
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

    public void Setup(Color newColor, int index)
    {
        color = newColor;
        colorIndex = index;

        ColorPreview.color = color;
    }
    
    private void HandleSelectedColor(int index, Color newColor)
    {
        // Only update THIS crayon
        if (index != colorIndex) return;

        if (LevelRuntime.Value.LevelGameMode == LevelGameMode.ColorPicker)
        {
            LevelRuntime.Value.SetWhiteColor(colorIndex, newColor);

            // GET the snapped color back
            color = LevelRuntime.Value.GetActiveColors()[colorIndex];
        }
        else
        {
            color = newColor;
        }

        ColorPreview.color = color;
    }
    
    private void OnClick()
    {
        _selectBrushColorEvent.Raise(colorIndex);
    }

    private void ExpandSize()
    {
        ResizeTweenScriptableObject.Expand(this.gameObject);
    }

    private void CollapseSize()
    {
        ResizeTweenScriptableObject.Collapse(this.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TryExpandAndShowRGB();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick(); // Normal click
            RGBSliderUI.Value.SetActive(false);
        }
    }
    
    private void TryExpandAndShowRGB()
    {
        if (LevelRuntime == null || LevelRuntime.Value == null) return;
        
        if (LevelRuntime.Value.LevelGameMode != LevelGameMode.ColorPicker) return;
        
        ExpandSize();
        SetColorBlobLook.SetShadowColor(SelectedColor);
        
        RGBSliderUI.Value.SetActive(true);

        // Auto-select this crayon for brushing
        _selectBrushColorEvent.Raise(colorIndex);
        
        // Tells ColorPickerUI to load this color
        _openColorPickerEvent.Raise(color);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ResizeTweenScriptableObject ResizeTweenScriptableObject;
    [SerializeField] private Button Button;
    [SerializeField] private Image ColorPreview;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    [SerializeField] private SetColorBlobLook SetColorBlobLook;
    [SerializeField] private ColorPickedEvent ColorPickedEvent;
    [SerializeField] private LevelConfigRuntimeAsset LevelRuntime;
    [SerializeField] private GameObjectValue RGBSliderUI;
    
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
        ColorPickedEvent.OnColorPicked += HandleColorPicked;
    }

    private void OnDisable()
    {
        SelectColorEvent.OnColorSelected -= HandleColorSelected;
        SelectColorEvent.OnEraseSelected -= HandleEraseSelected;
        ColorPickedEvent.OnColorPicked -= HandleColorPicked;
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

    public void Setup(Color newColor, int index)
    {
        color = newColor;
        colorIndex = index;

        ColorPreview.color = color;
    }
    
    private void HandleColorPicked(int index, Color newColor)
    {
        // Only update THIS crayon
        if (index != colorIndex) return;

        color = newColor;
        ColorPreview.color = newColor;

        // Update runtime WhiteColors so painting uses the new color
        if (LevelRuntime != null && 
            LevelRuntime.Value != null && 
            LevelRuntime.Value.LevelGameMode == LevelGameMode.ColorPicker)
        {
            LevelRuntime.Value.SetWhiteColor(colorIndex, newColor);
        }
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
        SelectColorEvent.Raise(colorIndex);
    }
}

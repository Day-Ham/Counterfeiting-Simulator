using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour, IPointerClickHandler
{
    [Header("Events")]
    [SerializeField] private OpenColorPickerEvent _openColorPickerEvent;
    [SerializeField] private SelectedColorEvent _selectedColorEvent;
    [SerializeField] private SelectBrushColorEvent _selectBrushColorEvent;
    
    [Header("References")]
    [SerializeField] private ResizeTweenUnitScriptableObject resizeTweenUnitScriptableObject;
    [SerializeField] private SetColorBlobLook SetColorBlobLook;
    [SerializeField] private ConfigRuntime RuntimeAsset;
    [SerializeField] private GameObjectValue RGBSliderUI;
    
    [Header("UI")]
    [SerializeField] private Button Button;
    [SerializeField] private Image ColorPreview;
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Shadow Color")]
    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnSelectedColor;
    
    private Color color;
    private int colorIndex;
    private bool isCollapsed = false;
    
    private void OnEnable()
    {
        _selectBrushColorEvent.OnColorSelected += HandleBrushColorSelected;
        _selectBrushColorEvent.OnEraseSelected += HandleEraseSelected;
        _selectedColorEvent.Register(HandleSelectedColor);
        
        GameState.OnGameFinished += CollapseAfterGameFinished;
    }

    private void OnDisable()
    {
        _selectBrushColorEvent.OnColorSelected -= HandleBrushColorSelected;
        _selectBrushColorEvent.OnEraseSelected -= HandleEraseSelected;
        _selectedColorEvent.Unregister(HandleSelectedColor);
        
        GameState.OnGameFinished -= CollapseAfterGameFinished;
    }
    
    private void HandleBrushColorSelected(int selectedColorIndex)
    {
        if (GameState.IsGameFinished)
        {
            CollapseAfterGameFinished();
            return;
        }
        
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
        if (GameState.IsGameFinished)
        {
            CollapseAfterGameFinished();
            return;
        }
        
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
        if (index != colorIndex) return;

        if (RuntimeAsset == null || !RuntimeAsset.HasValue) return;

        var colorList = RuntimeAsset.GetActiveColors();

        if (colorList == null || colorIndex < 0 || colorIndex >= colorList.Count) return;

        color = colorList[colorIndex];
        ColorPreview.color = color;
    }
    
    private void OnClick()
    {
        if (GameState.IsGameFinished) return;
        
        _selectBrushColorEvent.Raise(colorIndex);
    }

    private void ExpandSize()
    {
        resizeTweenUnitScriptableObject.Expand(rectTransform);
    }

    private void CollapseSize()
    {
        resizeTweenUnitScriptableObject.Collapse(rectTransform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameState.IsGameFinished) 
        {
            CollapseAfterGameFinished();
            return;
        }
        
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TryExpandAndShowRGB();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick(); // Normal click
            CollapseRGB();
        }
    }
    
    private void TryExpandAndShowRGB()
    {
        if (GameState.IsGameFinished) return;
        if (RuntimeAsset == null || !RuntimeAsset.HasValue) return;

        var colorsList = RuntimeAsset.GetActiveColors();
        if (colorsList == null || colorIndex >= colorsList.Count) return;

        // Only allow in ColorPicker mode if the runtime exposes it
        if (RuntimeAsset is LevelConfigRuntimeAsset levelRuntime && levelRuntime.Value.GameMode != LevelGameMode.ColorPicker) return;
        
        ExpandSize();
        SetColorBlobLook.SetShadowColor(SelectedColor);
        
        RGBSliderUI.Value.SetActive(true);

        // Auto-select this crayon for brushing
        _selectBrushColorEvent.Raise(colorIndex);
        
        // Tells ColorPickerUI to load this color
        _openColorPickerEvent.Raise(color);
    }
    
    private void CollapseRGB()
    {
        RGBSliderUI.Value.SetActive(false);
        _openColorPickerEvent.RaiseClosed();
    }
    
    private void CollapseAfterGameFinished()
    {
        if (isCollapsed) return;
        CollapseSize();
        SetColorBlobLook.SetShadowColor(UnSelectedColor);
        RGBSliderUI.Value.SetActive(false);
        isCollapsed = true;
    }
}

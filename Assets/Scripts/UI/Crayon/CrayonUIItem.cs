using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Events")]
    [SerializeField] private ColorEvent colorEvent;
    [SerializeField] private BoolEvent toggleColorPickerUIEvent;
    [SerializeField] private SelectedColorEvent selectedColorEvent;
    [SerializeField] private IntEvent selectBrushColorEvent;    
    [SerializeField] private VoidEvent eraserSelectEvent;
    
    [Header("References")]
    [SerializeField] private ResizeTweenUnitScriptableObject selectedResizeTweenUnitScriptableObject;
    [SerializeField] private ResizeTweenUnitScriptableObject hoverResizeTweenUnitScriptableObject;
    [SerializeField] private SetColorBlobLook setColorBlobLook;
    [SerializeField] private ConfigRuntime runtimeAsset;
    
    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image colorPreview;
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Shadow Color")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unSelectedColor;
    
    private Color _color;
    private int _colorIndex;
    private bool _isCollapsed = false;
    private int _currentSelectedColorIndex = -1;
    
    private void OnEnable()
    {
        selectBrushColorEvent.Register(HandleBrushColorSelected);
        eraserSelectEvent.Register(HandleEraseSelected);
        selectedColorEvent.Register(HandleSelectedColor);
        
        GameState.OnGameFinished += CollapseAfterGameFinished;
    }

    private void OnDisable()
    {
        selectBrushColorEvent.Unregister(HandleBrushColorSelected);
        eraserSelectEvent.Unregister(HandleEraseSelected);
        selectedColorEvent.Unregister(HandleSelectedColor);
        
        GameState.OnGameFinished -= CollapseAfterGameFinished;
    }
    
    private void HandleBrushColorSelected(int selectedColorIndex)
    {
        if (GameState.IsGameFinished)
        {
            CollapseAfterGameFinished();
            return;
        }
        
        _currentSelectedColorIndex = selectedColorIndex;
        
        if (selectedColorIndex == _colorIndex)
        {
            ExpandSize();
            setColorBlobLook.SetShadowColor(selectedColor);
        }
        else
        {
            CollapseSize();
            setColorBlobLook.SetShadowColor(unSelectedColor);
        }
    }
    
    private void HandleEraseSelected()
    {
        if (GameState.IsGameFinished)
        {
            CollapseAfterGameFinished();
            return;
        }
        
        _currentSelectedColorIndex = -1;
        CollapseSize();
        setColorBlobLook.SetShadowColor(unSelectedColor);
        toggleColorPickerUIEvent.Raise(false);
    }
        
    private void Awake()
    {
        CollapseSize();
        
        setColorBlobLook.SetShadowColor(unSelectedColor);
        
        button.onClick.AddListener(OnClick);
    }

    public void Setup(Color newColor, int index)
    {
        _color = newColor;
        _colorIndex = index;

        colorPreview.color = _color;
    }
    
    private void HandleSelectedColor(int index, Color newColor)
    {
        if (index != _colorIndex) return;

        if (runtimeAsset == null || !runtimeAsset.HasValue) return;

        var colorList = runtimeAsset.GetActiveColors();

        if (colorList == null || _colorIndex < 0 || _colorIndex >= colorList.Count) return;

        _color = colorList[_colorIndex];
        colorPreview.color = _color;
    }
    
    private void OnClick()
    {
        if (GameState.IsGameFinished) return;
        
        selectBrushColorEvent.Raise(_colorIndex);
    }

    private void ExpandSize()
    {
        selectedResizeTweenUnitScriptableObject.Expand(rectTransform);
    }

    private void CollapseSize()
    {
        selectedResizeTweenUnitScriptableObject.Collapse(rectTransform);
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
            CollapseRGBPicker();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if game is finished, don't change shadow color on hover
        if (GameState.IsGameFinished) return;
        // Only change shadow color on hover if this crayon is not already selected
        if (_colorIndex == _currentSelectedColorIndex) return;
        
        setColorBlobLook.SetShadowColor(Color.Lerp(unSelectedColor, selectedColor, 0.5f));
        hoverResizeTweenUnitScriptableObject.Expand(rectTransform);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_colorIndex == _currentSelectedColorIndex) return;
        setColorBlobLook.SetShadowColor(unSelectedColor);
        hoverResizeTweenUnitScriptableObject.Collapse(rectTransform);
    }

    private void TryExpandAndShowRGB()
    {
        if (GameState.IsGameFinished) return;
        if (runtimeAsset == null || !runtimeAsset.HasValue) return;

        var colorsList = runtimeAsset.GetActiveColors();
        if (colorsList == null || _colorIndex >= colorsList.Count) return;

        // Only allow in ColorPicker mode if the runtime exposes it
        if (runtimeAsset is MainGameConfigRuntimeAsset levelRuntime && levelRuntime.Value.GameMode != LevelGameMode.ColorPicker) return;
        
        ExpandSize();
        setColorBlobLook.SetShadowColor(selectedColor);

        // Auto-select this crayon for brushing
        selectBrushColorEvent.Raise(_colorIndex);
        
        // Tells ColorPickerUI to load this color
        colorEvent.Raise(_color);
        
        toggleColorPickerUIEvent.Raise(true);
    }
    
    private void CollapseRGBPicker()
    {
        toggleColorPickerUIEvent.Raise(false);
    }
    
    private void CollapseAfterGameFinished()
    {
        if (_isCollapsed) return;
        CollapseSize();
        CollapseRGBPicker();
        setColorBlobLook.SetShadowColor(unSelectedColor);
        _isCollapsed = true;
    }
}

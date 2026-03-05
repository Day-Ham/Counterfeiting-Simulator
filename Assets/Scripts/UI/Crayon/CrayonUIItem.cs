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
    
    [Header("Shadow Color")]
    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnSelectedColor;
    
    private LevelConfigRuntimeAsset _levelConfigRuntimeAsset;
    private Color color;
    private int colorIndex;
    
    private void OnEnable()
    {
        SelectColorEvent.OnColorSelected += HandleColorSelected;
        SelectColorEvent.OnEraseSelected += HandleEraseSelected;
    }

    private void OnDisable()
    {
        SelectColorEvent.OnColorSelected -= HandleColorSelected;
        SelectColorEvent.OnEraseSelected -= HandleEraseSelected;
    }
    
    private void HandleColorSelected(Color selected)
    {
        if (selected == color)
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

    public void Setup(Color newColor, LevelConfigRuntimeAsset levelConfig)
    {
        color = newColor;
        ColorPreview.color = color;
        _levelConfigRuntimeAsset = levelConfig;
        
        if (_levelConfigRuntimeAsset.Value.GameMode == ColorGameMode.ColorPicker)
        {
            SetColorBlobLook.SetMainColor(Color.white);
        }
    }
    
    private void OnClick()
    {
        SelectColorEvent.Raise(color);
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
            HandleRightClick();
        }
    }
    
    private void HandleRightClick()
    {
        if (_levelConfigRuntimeAsset == null) return;

        var levelConfig = _levelConfigRuntimeAsset.Value;
        if (levelConfig == null) return;

        if (levelConfig.GameMode == ColorGameMode.ColorPicker)
        {
            Debug.Log("Show ColorPicker UI");
        }
    }
}

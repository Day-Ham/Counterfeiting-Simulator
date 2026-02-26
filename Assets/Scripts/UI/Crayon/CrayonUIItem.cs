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

    public void Setup(Color newColor)
    {
        color = newColor;
        ColorPreview.color = color;
    }
    
    private void OnClick()
    {
        SelectColorEvent.Raise(color);
        Debug.Log($"Crayon clicked: {color}", this);
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

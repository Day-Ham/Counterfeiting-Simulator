using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private ResizeTweenScriptableObject ResizeTweenScriptableObject;
    [SerializeField] private Button Button;
    [SerializeField] private Image ColorPreview;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    [SerializeField] private SetColorBlobLook SetColorBlobLook;
    
    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnSelectedColor;
    
    private Color color;
    private static CrayonUIItem currentSelected;
        
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
        SelectThisCrayon();
        SelectColorEvent.Raise(color);
        Debug.Log($"Crayon clicked: {color}", this);
    }

    private void SelectThisCrayon()
    {
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.CollapseSize();
            
            currentSelected.SetColorBlobLook.SetShadowColor(currentSelected.UnSelectedColor);
        }
        
        ExpandSize();
        SetColorBlobLook.SetShadowColor(SelectedColor);

        currentSelected = this;
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

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private ResizeTweenScriptableObject ResizeTweenScriptableObject;
    [SerializeField] private Button Button;
    [SerializeField] private Image ColorPreview;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    
    private Color color;
    private static CrayonUIItem currentSelected;
        
    private void Awake()
    {
        CollapseSize();
        
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
        };

        ExpandSize();
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

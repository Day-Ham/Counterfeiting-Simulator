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
        ResizeTweenScriptableObject.Collapse(this.gameObject);
        
        Button.onClick.AddListener(Select);
    }

    public void Setup(Color newColor)
    {
        color = newColor;
        ColorPreview.color = color;
    }
    
    private void Select()
    {
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.ResizeTweenScriptableObject.Collapse(currentSelected.gameObject);
        }

        ResizeTweenScriptableObject.Expand(this.gameObject);
        
        SelectColorEvent.Raise(color);
        
        currentSelected = this;
        
        Debug.Log("Crayon clicked", this);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private Image colorPreview;
    [SerializeField] private Animator anim;
    [SerializeField] private SelectBrushColorEvent selectColorEvent;
    
    private Color color;

    public void Setup(Color newColor)
    {
        color = newColor;
        colorPreview.color = color;
    }
    
    public void Select()
    {
        anim?.Play("Selected");
        selectColorEvent.Raise(color);
    }
}

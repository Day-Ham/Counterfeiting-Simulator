using UnityEngine;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image colorPreview;
    [SerializeField] private Animator anim;
    [SerializeField] private SelectBrushColorEvent selectColorEvent;
    
    private Color color;
    
    private void Awake()
    {
        button.onClick.AddListener(Select);
    }

    public void Setup(Color newColor)
    {
        color = newColor;
        colorPreview.color = color;
        Debug.Log(color);
    }
    
    private void Select()
    {
        Debug.Log("Crayon clicked", this);
        
        anim?.Play("Selected");
        selectColorEvent.Raise(color);
    }
}

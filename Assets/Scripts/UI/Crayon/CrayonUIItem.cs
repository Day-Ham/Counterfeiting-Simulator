using UnityEngine;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Image ColorPreview;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    
    private Color color;
        
    private void Awake()
    {
        Button.onClick.AddListener(Select);
    }

    public void Setup(Color newColor)
    {
        color = newColor;
        ColorPreview.color = color;
        
        Debug.Log(color);
    }
    
    private void Select()
    {
        Debug.Log("Crayon clicked", this);
        
        SelectColorEvent.Raise(color);
    }
}

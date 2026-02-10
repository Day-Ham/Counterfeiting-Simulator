using UnityEngine;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private Button Button;
    //[SerializeField] private Image ShadowColorPreview;
    [SerializeField] private Image ColorPreview;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    
    private Color color;

    //private const float SHADOWCOLORALPHA = 0.5f;
        
    private void Awake()
    {
        Button.onClick.AddListener(Select);
    }

    public void Setup(Color newColor)
    {
        color = newColor;
        ColorPreview.color = color;
        
        /*Color shadowColor = color;
        shadowColor.a = SHADOWCOLORALPHA;
        ShadowColorPreview.color = shadowColor;*/
        
        Debug.Log(color);
    }
    
    private void Select()
    {
        Debug.Log("Crayon clicked", this);
        
        SelectColorEvent.Raise(color);
    }
}

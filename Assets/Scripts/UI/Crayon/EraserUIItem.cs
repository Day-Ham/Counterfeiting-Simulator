using UnityEngine;
using UnityEngine.UI;

public class EraserUIItem : MonoBehaviour
{
    [SerializeField] private ResizeTweenScriptableObject EraserTweenScriptableObject;
    [SerializeField] private Button Button;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    
    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
        
        Collapse();
    }
    
    private void OnEnable()
    {
        SelectColorEvent.OnEraseSelected += OnEraserSelected;
        SelectColorEvent.OnColorSelected += OnOtherColorSelected;
    }

    private void OnDisable()
    {
        SelectColorEvent.OnEraseSelected -= OnEraserSelected;
        SelectColorEvent.OnColorSelected -= OnOtherColorSelected;
    }

    private void OnClick()
    {
        SelectColorEvent.RaiseErase();
        Debug.Log("Eraser clicked", this);
    }
    
    private void OnEraserSelected()
    {
        Expand();
    }

    private void OnOtherColorSelected(Color _)
    {
        Collapse();
    }

    private void Expand()
    {
        EraserTweenScriptableObject.Expand(this.gameObject);
    }

    private void Collapse()
    {
        EraserTweenScriptableObject.Collapse(this.gameObject);
    }
}

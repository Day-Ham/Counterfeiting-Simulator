using UnityEngine;
using UnityEngine.UI;

public class EraserUIItem : MonoBehaviour
{
    [SerializeField] private ResizeTweenScriptableObject EraserTweenScriptableObject;
    [SerializeField] private Button Button;
    [SerializeField] private SelectBrushColorEvent SelectColorEvent;
    
    private bool isCollapsed = false;
    
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
    
    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
        
        Collapse();
    }

    private void Update()
    {
        if (!GameState.GameFinished || isCollapsed) return;
        Collapse();
        isCollapsed = true;
    }

    private void OnClick()
    {
        if (GameState.GameFinished) return;
        
        SelectColorEvent.RaiseErase();
        Debug.Log("Eraser clicked", this);
    }
    
    private void OnEraserSelected()
    {
        if (!GameState.GameFinished) Expand();
    }

    private void OnOtherColorSelected(int _)
    {
        if (!GameState.GameFinished) Collapse();
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

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
        
        GameState.OnGameFinished += CollapseAfterGameFinished;
    }

    private void OnDisable()
    {
        SelectColorEvent.OnEraseSelected -= OnEraserSelected;
        SelectColorEvent.OnColorSelected -= OnOtherColorSelected;
        
        GameState.OnGameFinished -= CollapseAfterGameFinished;
    }
    
    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
        
        Collapse();
    }

    private void OnClick()
    {
        if (GameState.IsGameFinished) return;
        
        SelectColorEvent.RaiseErase();
        Debug.Log("Eraser clicked", this);
    }
    
    private void OnEraserSelected()
    {
        if (!GameState.IsGameFinished) Expand();
    }

    private void OnOtherColorSelected(int _)
    {
        if (!GameState.IsGameFinished) Collapse();
    }

    private void Expand()
    {
        EraserTweenScriptableObject.Expand(this.gameObject);
    }

    private void Collapse()
    {
        EraserTweenScriptableObject.Collapse(this.gameObject);
    }
    
    private void CollapseAfterGameFinished()
    {
        if (isCollapsed) return;
        Collapse();
        isCollapsed = true;
    }
}

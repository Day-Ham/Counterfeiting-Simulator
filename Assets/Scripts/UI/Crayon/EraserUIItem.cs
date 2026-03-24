using UnityEngine;
using UnityEngine.UI;

public class EraserUIItem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private SelectBrushColorEvent selectColorEvent;
    
    [Header("UI")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;
    
    [Header("ResizeTween")]
    [SerializeField] private ResizeTweenUnitScriptableObject eraserTweenUnitScriptableObject;
    
    private bool _isCollapsed = false;
    
    private void OnEnable()
    {
        selectColorEvent.OnEraseSelected += OnEraserSelected;
        selectColorEvent.OnColorSelected += OnOtherColorSelected;
        
        GameState.OnGameFinished += CollapseAfterGameFinished;
    }

    private void OnDisable()
    {
        selectColorEvent.OnEraseSelected -= OnEraserSelected;
        selectColorEvent.OnColorSelected -= OnOtherColorSelected;
        
        GameState.OnGameFinished -= CollapseAfterGameFinished;
    }
    
    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        
        Collapse();
    }

    private void OnClick()
    {
        if (GameState.IsGameFinished) return;
        
        selectColorEvent.RaiseErase();
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
        eraserTweenUnitScriptableObject.Expand(rectTransform);
    }

    private void Collapse()
    {
        eraserTweenUnitScriptableObject.Collapse(rectTransform);
    }
    
    private void CollapseAfterGameFinished()
    {
        if (_isCollapsed) return;
        Collapse();
        _isCollapsed = true;
    }
}

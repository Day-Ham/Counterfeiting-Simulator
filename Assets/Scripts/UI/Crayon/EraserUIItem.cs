using UnityEngine;
using UnityEngine.UI;

public class EraserUIItem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private IntEvent colorSelectedEvent;
    [SerializeField] private VoidEvent eraserSelectEvent;
    
    [Header("UI")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;
    
    [Header("ResizeTween")]
    [SerializeField] private ResizeTweenUnitScriptableObject eraserTweenUnitScriptableObject;
    
    private bool _isCollapsed = false;
    
    private void OnEnable()
    {
        eraserSelectEvent.Register(OnEraserSelected);
        colorSelectedEvent.Register(OnOtherColorSelected);
        
        GameState.OnGameFinished += CollapseAfterGameFinished;
    }

    private void OnDisable()
    {
        eraserSelectEvent.Unregister(OnEraserSelected);
        colorSelectedEvent.Unregister(OnOtherColorSelected);
        
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
        
        eraserSelectEvent.Raise();
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
        eraserTweenUnitScriptableObject.Play(rectTransform);
    }

    private void Collapse()
    {
        eraserTweenUnitScriptableObject.PlayReverse(rectTransform);
    }
    
    private void CollapseAfterGameFinished()
    {
        if (_isCollapsed) return;
        Collapse();
        _isCollapsed = true;
    }
}

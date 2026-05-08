using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EraserUIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Events")]
    [SerializeField] private IntEvent colorSelectedEvent;
    [SerializeField] private VoidEvent eraserSelectEvent;
    [SerializeField] private AudioClipEvent audioClipEvent;
    
    [Header("UI")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;

    [Header("SFX")]
    [SerializeField] private AudioClipValue eraserSelectSFX;
    [SerializeField] private AudioClipValue eraserHoverSFX;
    
    [Header("ResizeTween")]
    [SerializeField] private ResizeTweenUnitScriptableObject eraserSelectedTweenUnitScriptableObject;
    [SerializeField] private ResizeTweenUnitScriptableObject eraserHoverTweenUnitScriptableObject;

    
    private bool _isCollapsed = false;
    private bool _isSelected = false;
    
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
        
        Collapse(eraserSelectedTweenUnitScriptableObject);
    }

    private void OnClick()
    {
        if (GameState.IsGameFinished) return;
        
        audioClipEvent.Raise(eraserSelectSFX.Value);
        eraserSelectEvent.Raise();
    }
    
    private void OnEraserSelected()
    {
        if (!GameState.IsGameFinished) 
        {
            _isSelected = true;
            Expand(eraserSelectedTweenUnitScriptableObject);
        }
    }

    private void OnOtherColorSelected(int _)
    {
        if (!GameState.IsGameFinished)
        {
            _isSelected = false;
            Collapse(eraserSelectedTweenUnitScriptableObject);
        }
    }

    private void Expand(ResizeTweenUnitScriptableObject tweenUnitScriptableObject)
    {
        tweenUnitScriptableObject.Play(rectTransform);
    }

    private void Collapse(ResizeTweenUnitScriptableObject tweenUnitScriptableObject)
    {
        tweenUnitScriptableObject.PlayReverse(rectTransform);
    }

    private void CollapseAfterGameFinished()
    {
        if (_isCollapsed) return;
        Collapse(eraserSelectedTweenUnitScriptableObject);
        button.interactable = false;
        _isCollapsed = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameState.IsGameFinished || _isSelected) return;

        Expand(eraserHoverTweenUnitScriptableObject);
        audioClipEvent.Raise(eraserHoverSFX.Value);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameState.IsGameFinished || _isSelected) return;

        Collapse(eraserHoverTweenUnitScriptableObject);
    }
}

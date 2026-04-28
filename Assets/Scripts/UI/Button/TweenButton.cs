using UnityEngine;
using UnityEngine.EventSystems;

public class TweenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RelativeRescaleTweenUnitScriptableObject hoverTween;
    [SerializeField] private RelativeRescaleTweenUnitScriptableObject pressTween;
    [SerializeField] private RectTransform target;

    private Vector3 _originalScale;

    private void Awake()
    {
        if (target) _originalScale = target.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData) => hoverTween?.Expand(target, _originalScale);
    public void OnPointerExit(PointerEventData eventData) => hoverTween?.Collapse(target, _originalScale);
    public void OnPointerDown(PointerEventData eventData) => pressTween?.Expand(target, _originalScale);
    public void OnPointerUp(PointerEventData eventData) => pressTween?.Collapse(target, _originalScale);
}

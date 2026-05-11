using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleAnimationEventListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BoolEvent toggleEvent;
    [SerializeField] private TweenAnimationUnitScriptable toggledAnimation;
    [SerializeField] private TweenAnimationUnitScriptable hoverAnimation;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 startScale = Vector3.zero;

    private RectTransform _targetRect;
    private bool _isOn;

    private void Awake() => _targetRect = target.GetComponent<RectTransform>();
    private void Start() => _targetRect.localScale = startScale;
    private void OnEnable() => toggleEvent.Register(OnToggleEvent);
    private void OnDisable() => toggleEvent.Unregister(OnToggleEvent);

    private void OnToggleEvent(bool value)
    {
        _isOn = value;
        if (value) toggledAnimation.Play(_targetRect);
        else toggledAnimation.PlayReverse(_targetRect);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isOn || hoverAnimation == null) return;
        hoverAnimation.Play(_targetRect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isOn || hoverAnimation == null) return;
        hoverAnimation.PlayReverse(_targetRect);
    }
}

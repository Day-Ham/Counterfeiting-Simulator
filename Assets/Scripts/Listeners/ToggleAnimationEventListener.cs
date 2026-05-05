using DG.Tweening;
using UnityEngine;

public class ToggleAnimationEventListener : MonoBehaviour
{
    [SerializeField] private BoolEvent toggleEvent;
    [SerializeField] private TweenAnimationUnitScriptable tweenAnimation;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 startScale = Vector3.zero;

    private RectTransform _targetRect;

    private void Awake() => _targetRect = target.GetComponent<RectTransform>();
    private void Start() => _targetRect.localScale = startScale;
    private void OnEnable() => toggleEvent.Register(OnToggleEvent);
    private void OnDisable() => toggleEvent.Unregister(OnToggleEvent);

    private void OnToggleEvent(bool value)
    {
        if (value)
        {
            tweenAnimation.Play(_targetRect, null);
        }
        else
        {
            tweenAnimation.PlayReverse(_targetRect);
        }
    }
}

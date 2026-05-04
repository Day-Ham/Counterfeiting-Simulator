using UnityEngine;

public class ToggleAnimationEventListener : MonoBehaviour
{
    [SerializeField] private BoolEvent toggleEvent;
    [SerializeField] private RescaleTweenUnitScriptableObject tweenAnimation;
    [SerializeField] private GameObject target;

    private RectTransform _targetRect;

    private void Awake() => _targetRect = target.GetComponent<RectTransform>();
    private void Start() => _targetRect.localScale = Vector3.zero;
    private void OnEnable() => toggleEvent.Register(OnToggleEvent);
    private void OnDisable() => toggleEvent.Unregister(OnToggleEvent);

    private void OnToggleEvent(bool value)
    {
        if (value)
        {
            target.SetActive(true);
            tweenAnimation.Expand(_targetRect, null);
        }
        else
        {
            tweenAnimation.Collapse(_targetRect, () => target.SetActive(false));
        }
    }
}

using UnityEngine;

public class MoveEventListener : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private VoidEvent moveEvent;

    [Header("Tween")]
    [SerializeField] private MoveTweenUnitScriptableObject moveTween;

    [Header("Target")]
    [SerializeField] private RectTransform rectTransform;

    private void OnEnable() => moveEvent.Register(OnMoveEvent);
    private void OnDisable() => moveEvent.Unregister(OnMoveEvent);

    private void OnMoveEvent() => moveTween.Play(rectTransform);
}

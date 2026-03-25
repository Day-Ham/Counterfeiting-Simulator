using UnityEngine;

public class TweenAnimationHandler : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent triggerEvent;
    
    [Header("UI")]
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Tween Animation")]
    [SerializeField] private TweenAnimationUnitScriptable tweenAnimationUnitScriptable;

    private void OnEnable()
    {
        triggerEvent.Register(OnEventTriggered);
    }

    private void OnDisable()
    {
        triggerEvent.Unregister(OnEventTriggered);
    }

    private void OnEventTriggered()
    {
        tweenAnimationUnitScriptable?.Play(rectTransform);
    }

    public void StopTween()
    {
        tweenAnimationUnitScriptable?.Stop(rectTransform);
    }
}

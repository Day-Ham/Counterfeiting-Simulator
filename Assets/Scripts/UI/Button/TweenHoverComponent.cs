using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTweenComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TweenAnimationUnitScriptable hoverAnimation;
    [SerializeField] private RectTransform target;

    public void OnPointerEnter(PointerEventData eventData) => hoverAnimation.Play(target);
    public void OnPointerExit(PointerEventData eventData) => hoverAnimation.PlayReverse(target);
}

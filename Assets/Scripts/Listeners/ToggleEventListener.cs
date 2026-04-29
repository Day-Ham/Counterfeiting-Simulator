using UnityEngine;

public class ToggleEventListener : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private BoolEvent toggleEvent;

    [Header("Tween")]
    [SerializeField] private RescaleTweenUnitScriptableObject tweenAnimation;
    [SerializeField] private bool playAnimation;

    [Header("Target GameObject")]
    [SerializeField] private GameObject target;

    private void OnEnable()
    {
        toggleEvent.Register(OnToggleEvent);
    }

    private void OnDisable()
    {
        toggleEvent.Unregister(OnToggleEvent);
    }

    private void OnToggleEvent(bool value)
    {
        if(playAnimation && tweenAnimation != null)
        {
            if(value)
            {
                tweenAnimation.Expand(target.GetComponent<RectTransform>(), null);                
            }
            else
            {
                tweenAnimation.Collapse(target.GetComponent<RectTransform>(), null);
            }
        }
        // if (target)
        // {
        //     target.SetActive(value);
        // }
    }
}

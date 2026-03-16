using DG.Tweening;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] protected GameObjectValue circleTransition;

    [Header("Tween Settings")]
    [SerializeField] protected float duration = 1f;
    [SerializeField] protected Ease ease = Ease.OutQuad;

    private GameObject CircleUI => circleTransition.Value;
    
    private bool isOpened;

    protected virtual void Start()
    {
        if (isOpened) return;

        isOpened = true;
        
        CircleUI.SetActive(true);
        CircleUI.transform.localScale = Vector3.one * 25f;
        
        CircleUI.transform
            .DOScale(Vector3.zero, duration)
            .SetEase(ease);
    }

    public void PlayCloseTransition(System.Action onComplete)
    {
        CircleUI.transform
            .DOScale(Vector3.one * 25f, duration)
            .SetEase(ease)
            .OnComplete(() => onComplete?.Invoke());
    }
}

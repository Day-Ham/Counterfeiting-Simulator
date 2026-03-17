using DG.Tweening;
using UnityEngine;

public class UITransitionElement : MonoBehaviour
{
    [SerializeField] private UITransitionManagerValue managerValue;

    [SerializeField] private float moveDistance = 1200f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.InBack;

    private RectTransform rect;
    private Vector2 originalPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }

    private void OnEnable()
    {
        managerValue?.Register(this);
    }

    private void OnDisable()
    {
        managerValue?.Unregister(this);
    }

    public void MoveOut()
    {
        Vector2 dir = GetDirectionFromAnchor();
        Vector2 target = originalPos + dir * moveDistance;

        rect.DOAnchorPos(target, duration)
            .SetEase(ease)
            .SetUpdate(true);
    }

    public void MoveIn()
    {
        rect.DOAnchorPos(originalPos, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    private Vector2 GetDirectionFromAnchor()
    {
        Vector2 anchorCenter = (rect.anchorMin + rect.anchorMax) / 2f;

        Vector2 dir = Vector2.zero;

        if (anchorCenter.x < 0.5f) dir.x = -1;
        else if (anchorCenter.x > 0.5f) dir.x = 1;

        if (anchorCenter.y < 0.5f) dir.y = -1;
        else if (anchorCenter.y > 0.5f) dir.y = 1;

        return dir.normalized;
    }
}

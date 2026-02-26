using DG.Tweening;
using UnityEngine;

public class TestTween : MonoBehaviour
{
    private RectTransform rectTransform;
    public Ease ease = Ease.OutBounce;
    public float duration = 1f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        MoveUpByTen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveUpByTen();
        }
    }
    [ContextMenu("Move Up")]
    public void MoveUpByTen()
    {
        //rectTransform.anchoredPosition = Vector2.zero; // Reset position before moving
        rectTransform.DOAnchorPos(Vector2.up * 150, duration)
            .SetRelative()       // This makes it "move by" instead of "move to"
            .SetEase(ease);
       // rectTransform.DOAnchorPos(Vector2.up * -150, duration);
    }
}

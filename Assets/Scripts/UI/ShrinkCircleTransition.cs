using DG.Tweening;
using UnityEngine;

public class ShrinkCircleTransition : MonoBehaviour
{
    public GameObjectValue CircleTransition;

    private GameObject CircleUI => CircleTransition.Value;
    private float CircleScaleMultiplier = 25f;

    private void Start()
    {
        // Make it huge and visible
        CircleUI.SetActive(true);
        CircleUI.transform.localScale = Vector3.one * CircleScaleMultiplier;

        // Shrink to normal
        CircleUI.transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // Optionally hide after shrink
                CircleUI.SetActive(false);
            });
    }
}

using UnityEngine;

public class StartUIScale : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float startingScaleX;
    [SerializeField] private float startingScaleY;

    private void Awake()
    {
        rectTransform.localScale = new Vector3(startingScaleX, startingScaleY, rectTransform.localScale.z);
    }
}

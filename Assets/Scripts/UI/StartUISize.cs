using UnityEngine;

public class StartUISize : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float startingSizeX;
    [SerializeField] private float startingSizeY;

    private void Awake()
    {
        rectTransform.sizeDelta = new Vector2(startingSizeX, startingSizeY);
    }
}

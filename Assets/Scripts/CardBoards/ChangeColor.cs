using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Image backgroundImage;

    void Start()
    {
        if (targetImage != null)
        {
            targetImage.color = Color.white;
        }
    }
}
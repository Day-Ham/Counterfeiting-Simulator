using UnityEngine;
using UnityEngine.UI;

public class CrayonUIItem : MonoBehaviour
{
    [SerializeField] private Image colorPreview;

    public void Setup(Color color)
    {
        colorPreview.color = color;
    }
}

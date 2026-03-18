using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuctionGalleryItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image drawingImage;
    [SerializeField] private TextMeshProUGUI bidText;

    private readonly Vector2 _pivot = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// Sets the drawing and bid
    /// </summary>
    public void SetData(byte[] drawingData, int finalPrice)
    {
        if (drawingData is { Length: > 0 })
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(drawingData);

            if (drawingImage != null)
            {
                drawingImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), _pivot);
                drawingImage.preserveAspect = true;
            }
        }

        if (bidText != null)
        {
            bidText.SetText("$" + finalPrice.ToString("n0"));
        }
    }
}

using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AuctionGalleryItem : MonoBehaviour, IPointerClickHandler
{
    [Header("Event")]
    [SerializeField] private BoolEvent toggleContextMenu;
    [SerializeField] private ByteArrayEvent selectedImageEvent;
    
    [Header("UI References")]
    [SerializeField] private Image drawingImage;
    [SerializeField] private TextMeshProUGUI bidText;
    [SerializeField] private TextMeshProUGUI paintingName;
    
    private byte[] _drawingData;
    private readonly Vector2 _pivot = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// Sets the drawing and bid
    /// </summary>
    public void SetData(byte[] drawingData, int finalPrice, string paintingNameValue)
    {
        _drawingData = drawingData;

        if (drawingData != null && drawingData.Length > 0)
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

        if (paintingName == null) return;
        
        string finalName = string.IsNullOrEmpty(paintingNameValue) ? "Untitled" : paintingNameValue;
        paintingName.SetText(finalName);
    }
    
    // Detect right-click
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
        
        toggleContextMenu.Raise(true);
        selectedImageEvent.Raise(_drawingData);
    }
}

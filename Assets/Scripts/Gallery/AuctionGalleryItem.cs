using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AuctionGalleryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Event")]
    [SerializeField] private BoolEvent toggleContextMenu;
    [SerializeField] private ByteArrayEvent selectedImageEvent;
    [SerializeField] private AuctionSavedDataEvent selectedSavedDataEvent;
    
    [Header("UI References")]
    [SerializeField] private Image drawingImage;
    [SerializeField] private TextMeshProUGUI bidText;
    [SerializeField] private TextMeshProUGUI paintingName;
    [SerializeField] private RectTransform cornerBadge;
    [SerializeField] private FadeTweenUnitScriptableObject fadeTween;
    
    private AuctionSavedData _auctionSavedData;
    private readonly Vector2 _pivot = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// Sets the drawing and bid
    /// </summary>
    public void SetData(AuctionSavedData savedData)
    {
        _auctionSavedData = savedData;

        if (savedData.drawingData != null && savedData.drawingData.Length > 0)
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(savedData.drawingData);

            drawingImage.sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                _pivot
            );

            drawingImage.preserveAspect = true;
        }

        bidText.SetText("$" + savedData.finalPrice.ToString("n0"));

        paintingName.SetText(
            string.IsNullOrEmpty(savedData.paintingName)
                ? "Untitled"
                : savedData.paintingName
        );
    }
    
    // Detect right-click
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
        
        toggleContextMenu.Raise(true);
        selectedImageEvent.Raise(_auctionSavedData.drawingData);
        selectedSavedDataEvent.Raise(_auctionSavedData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        fadeTween.Play(cornerBadge, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        fadeTween.PlayReverse(cornerBadge, null);
    }
}

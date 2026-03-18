using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuctionLoadHandler : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private Image displayImage;
    [SerializeField] private Vector2 pivot = new Vector2(0.5f, 0.5f);
    [SerializeField] private TextMeshProUGUI bidText;

    private const string saveFileName = "AuctionSave.es3";

    private void Start()
    {
        LoadAndDisplayLastAuction();
    }

    /// <summary>
    /// Loads the last auction drawing and bid and displays them.
    /// </summary>
    private void LoadAndDisplayLastAuction()
    {
        ES3Settings settings = new ES3Settings(saveFileName);

        if (ES3.KeyExists("Auction_Drawing", settings) && ES3.KeyExists("Auction_FinalPrice", settings))
        {
            byte[] drawingData = ES3.Load<byte[]>("Auction_Drawing", settings);
            int finalPrice = ES3.Load<int>("Auction_FinalPrice", settings);

            // Convert PNG bytes to Texture2D
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(drawingData);

            Debug.Log($"[AuctionLoadHandler] Loaded auction successfully! Price: ${finalPrice}, Image size: {drawingData.Length / 1024} KB");

            // Display drawing
            if (displayImage != null)
            {
                displayImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot);
                Debug.Log("[AuctionLoadHandler] Drawing displayed on renderer!");
            }

            // Display bid
            if (bidText != null)
            {
                bidText.SetText("$" + finalPrice.ToString("n0"));
                Debug.Log("[AuctionLoadHandler] Bid displayed in TMP!");
            }
        }
        else
        {
            Debug.LogWarning("[AuctionLoadHandler] No saved auction found.");
        }
    }
}

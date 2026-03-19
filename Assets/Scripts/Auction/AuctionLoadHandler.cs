using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuctionLoadHandler : MonoBehaviour
{
    [Header("Gallery")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject auctionItemPrefab;

    private const string SaveFileName = "AuctionSave.es3";

    private void Start()
    {
        LoadAllAuctions();
    }

    private void LoadAllAuctions()
    {
        ES3Settings settings = new ES3Settings(SaveFileName);

        if (!ES3.KeyExists("Auction_History", settings))
        {
            Debug.LogWarning("[AuctionLoadHandler] No saved auctions found.");
            return;
        }

        List<AuctionSavedData> history = ES3.Load<List<AuctionSavedData>>("Auction_History", settings);
        Debug.Log($"[AuctionLoadHandler] Loading {history.Count} auctions");

        foreach (var auctionSavedDate in history)
        {
            GameObject galleryImageUI = Instantiate(auctionItemPrefab, contentParent);
            AuctionGalleryItem galleryItem = galleryImageUI.GetComponent<AuctionGalleryItem>();

            if (galleryItem != null)
            {
                galleryItem.SetData(auctionSavedDate.DrawingData, auctionSavedDate.FinalPrice, auctionSavedDate.PaintingName);
            }
            else
            {
                Debug.LogWarning("[AuctionLoadHandler] Prefab missing AuctionGalleryItem script!");
            }
        }
    }
}

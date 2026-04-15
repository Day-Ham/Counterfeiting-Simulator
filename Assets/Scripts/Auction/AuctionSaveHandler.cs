using System;
using System.Collections.Generic;
using UnityEngine;

public class AuctionSaveHandler : MonoBehaviour
{
    [SerializeField] private AuctionResultRuntime auctionResult;

    private const string SaveFileName = "AuctionSave.es3";
    private const string AutoScrollFlagKey = "GalleryAutoScroll";

    /// <summary>
    /// Save the final auction drawing and price to history.
    /// </summary>
    public void Save()
    {
        if (auctionResult.drawingData == null)
        {
            Debug.LogError("[Save] No drawing data to save!");
            return;
        }

        ES3Settings settings = new ES3Settings(SaveFileName);

        List<AuctionSavedData> history;

        if (ES3.KeyExists("Auction_History", settings))
        {
            history = ES3.Load<List<AuctionSavedData>>("Auction_History", settings);
        }
        else
        {
            history = new List<AuctionSavedData>();
        }

        AuctionSavedData newEntry = new AuctionSavedData
        {
            paintingID = auctionResult.paintingID,
            drawingData = auctionResult.drawingData,
            finalPrice = auctionResult.finalPrice,
            paintingName = auctionResult.paintingName
        };

        history.Add(newEntry);

        ES3.Save("Auction_History", history, settings);

        // Save flag for gallery detection
        ES3.Save(AutoScrollFlagKey, true, settings);

        Debug.Log($"[Save] Auction saved successfully! Total auctions: {history.Count}");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class AuctionSaveHandler : MonoBehaviour
{
    [SerializeField] private AuctionResultRuntime auctionResult;

    private const string SaveFileName = "AuctionSave.es3";

    /// <summary>
    /// Save the final auction drawing and price to history.
    /// Each auction is appended to the list.
    /// </summary>
    public void Save()
    {
        if (auctionResult.DrawingData == null)
        {
            Debug.LogError("[Save] No drawing data to save!");
            return;
        }

        ES3Settings settings = new ES3Settings(SaveFileName);

        // Load existing history if exists, otherwise create new
        List<AuctionSavedData> history;
        if (ES3.KeyExists("Auction_History", settings))
        {
            history = ES3.Load<List<AuctionSavedData>>("Auction_History", settings);
        }
        else
        {
            history = new List<AuctionSavedData>();
        }

        // Append new entry
        AuctionSavedData newEntry = new AuctionSavedData
        {
            DrawingData = auctionResult.DrawingData,
            FinalPrice = auctionResult.FinalPrice,
            PaintingName = auctionResult.PaintingName
        };
        history.Add(newEntry);

        // Save updated history
        ES3.Save("Auction_History", history, settings);

        Debug.Log($"[Save] Auction saved successfully! Total auctions: {history.Count}");
        Debug.Log($"[Save] Final Price: ${auctionResult.FinalPrice}");
        Debug.Log($"[Save] Image Size: {auctionResult.DrawingData.Length / 1024} KB");
    }
}

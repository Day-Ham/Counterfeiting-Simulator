using System;
using System.Collections.Generic;
using UnityEngine;

public class AuctionSaveHandler : MonoBehaviour
{
    [SerializeField] private AuctionResultRuntime auctionResult;

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

        ES3Settings settings = new ES3Settings(SaveFileUtility.SAVE_FILE_NAME);

        List<AuctionSavedData> history;

        if (ES3.KeyExists(SaveFileUtility.SAVE_FILE_HISTORY, settings))
        {
            history = ES3.Load<List<AuctionSavedData>>(SaveFileUtility.SAVE_FILE_HISTORY, settings);
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

        ES3.Save(SaveFileUtility.SAVE_FILE_HISTORY, history, settings);

        // Save flag for gallery detection
        ES3.Save(SaveFileUtility.AUTO_SCROLL_FLAG_KEY, true, settings);

        Debug.Log($"[Save] Auction saved successfully! Total auctions: {history.Count}");
    }
}

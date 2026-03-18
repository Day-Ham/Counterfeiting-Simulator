using UnityEngine;
using System.IO;

public class AuctionSaveHandler : MonoBehaviour
{
    [SerializeField] private AuctionResultRuntime auctionResult;

    public void Save()
    {
        if (auctionResult.DrawingData == null)
        {
            Debug.LogError("[Save] No drawing data to save!");
            return;
        }

        ES3Settings settings = new ES3Settings("AuctionSave.es3");
        
        ES3.Save("Auction_Drawing", auctionResult.DrawingData, settings);
        ES3.Save("Auction_FinalPrice", auctionResult.FinalPrice, settings);
        
        string path = Path.Combine(Application.persistentDataPath, "ES3Files", "AuctionSave.es3");
        
        int dataSizeKB = auctionResult.DrawingData.Length / 1024;
        Debug.Log($"[Save] Auction saved successfully!");
        Debug.Log($"[Save] Path: {path}");
        Debug.Log($"[Save] Price: {auctionResult.FinalPrice}");
        Debug.Log($"[Save] Image Size: {dataSizeKB} KB");

        if (ES3.KeyExists("Auction_Drawing", settings))
        {
            Debug.Log("[Save] Drawing key exists ✔");
        }
        else
        {
            Debug.LogWarning("[Save] Drawing key does not exist yet (fresh save?)");
        }
    }
}

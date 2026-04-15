using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugAuctionSaveGenerator : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private int generateCount = 5;
    
    private const string SAVE_FILE_NAME = "AuctionSave.es3";
    private const string SAVE_KEY = "Auction_History";
    
    public void GenerateDebugSaves()
    { 
        ES3Settings settings = new ES3Settings(SAVE_FILE_NAME);
    
        List<AuctionSavedData> history;
    
        if (ES3.KeyExists(SAVE_KEY, settings))
            history = ES3.Load<List<AuctionSavedData>>(SAVE_KEY, settings);
        else
            history = new List<AuctionSavedData>();
    
        for (int i = 0; i < generateCount; i++)
        { 
            AuctionSavedData fakeData = new AuctionSavedData
            {
                drawingData = GenerateDummyImage(), // fake image
                finalPrice = Random.Range(50, 5000),
                paintingName = GetRandomName()
            };
    
            history.Add(fakeData);
        }
    
        ES3.Save(SAVE_KEY, history, settings);
    
        Debug.Log($"[DEBUG] Generated {generateCount} fake auction entries.");
    }
    
    //Create a simple colored texture > convert to PNG bytes
    private byte[] GenerateDummyImage()
    {
        Texture2D tex = new Texture2D(64, 64);
    
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        for (int x = 0; x < tex.width; x++) 
        { 
            for (int y = 0; y < tex.height; y++) 
            { 
                tex.SetPixel(x, y, randomColor);
            }
        }
        
        tex.Apply();
    
        return tex.EncodeToPNG();
    }
    
    private string GetRandomName()
    { 
        string[] names =
        {
            "Sunset Dreams", 
            "Void Echo", 
            "Neon Silence", 
            "Lost Memory", 
            "Fragment 01", 
            "Urban Bloom", 
            "Digital Ghost"
        };
        
        return names[Random.Range(0, names.Length)];
    }
}

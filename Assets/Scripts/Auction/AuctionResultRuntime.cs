using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AuctionResult", menuName = "Auction/AuctionResultRuntime")]
public class AuctionResultRuntime : ScriptableObject
{
    [HideInInspector] public string paintingID;
    [HideInInspector] public byte[] drawingData;
    [HideInInspector] public string paintingName;
    [HideInInspector] public int finalPrice;

    public void SetData(byte[] bytDrawingData, int intFinalPrice, string stringPaintingName)
    {
        paintingID = Guid.NewGuid().ToString();
        drawingData = bytDrawingData;
        finalPrice = intFinalPrice;
        paintingName = stringPaintingName;
    }

    
}

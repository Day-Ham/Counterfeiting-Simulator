using UnityEngine;

[CreateAssetMenu(fileName = "AuctionResult", menuName = "Auction/AuctionResultRuntime")]
public class AuctionResultRuntime : ScriptableObject
{
    [HideInInspector] public byte[] drawingData;
    [HideInInspector] public string paintingName;
    [HideInInspector] public int finalPrice;

    public void SetData(byte[] bytDrawingData, int intFinalPrice, string stringPaintingName)
    {
        drawingData = bytDrawingData;
        finalPrice = intFinalPrice;
        paintingName = stringPaintingName;
    }

    public void Clear()
    {
        drawingData = null;
        finalPrice = 0;
        paintingName = "";
    }
}

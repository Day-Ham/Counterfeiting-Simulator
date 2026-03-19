using UnityEngine;

[CreateAssetMenu(fileName = "AuctionResult", menuName = "Auction/AuctionResultRuntime")]
public class AuctionResultRuntime : ScriptableObject
{
    public byte[] DrawingData;
    public string PaintingName;
    public int FinalPrice;

    public void SetData(byte[] drawingData, int finalPrice, string paintingName)
    {
        DrawingData = drawingData;
        FinalPrice = finalPrice;
        PaintingName = paintingName;
    }

    public void Clear()
    {
        DrawingData = null;
        FinalPrice = 0;
        PaintingName = "";
    }
}

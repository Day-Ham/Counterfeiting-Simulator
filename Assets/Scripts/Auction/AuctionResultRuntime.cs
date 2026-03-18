using UnityEngine;

[CreateAssetMenu(fileName = "AuctionResult", menuName = "Auction/AuctionResultRuntime")]
public class AuctionResultRuntime : ScriptableObject
{
    public byte[] DrawingData;
    public int FinalPrice;

    public void SetData(byte[] drawingData, int finalPrice)
    {
        DrawingData = drawingData;
        FinalPrice = finalPrice;
    }

    public void Clear()
    {
        DrawingData = null;
        FinalPrice = 0;
    }
}

using UnityEngine;

public struct BidResultStruct
{
    public bool Success;
    public NPCBidderRuntime Bidder;
    public int BidAmount;
    public int NewPrice;

    public static BidResultStruct Fail()
    {
        return new BidResultStruct() { Success = false };
    }
}

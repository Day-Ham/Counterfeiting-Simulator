using UnityEngine;

public struct BidResultStruct
{
    public bool success;
    public NPCBidderRuntime bidder;
    public int bidAmount;
    public int newPrice;

    public static BidResultStruct Fail()
    {
        return new BidResultStruct() { success = false };
    }
}

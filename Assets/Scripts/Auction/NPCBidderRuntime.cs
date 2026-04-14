[System.Serializable]
public class NPCBidderRuntime
{
    public NPCBidder data;
    public int currentMoney;
    
    public BidderUIBinder bidderUIBinder;

    public NPCBidderRuntime(NPCBidder bidder)
    {
        data = bidder;
        currentMoney = bidder.maxMoney;
    }

    public bool CanBid(int currentPrice)
    {
        return currentMoney > currentPrice;
    }
}

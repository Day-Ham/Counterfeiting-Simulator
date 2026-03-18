[System.Serializable]
public class NPCBidderRuntime
{
    public NPCBidder Data;
    public int CurrentMoney;

    public NPCBidderRuntime(NPCBidder bidder)
    {
        Data = bidder;
        CurrentMoney = bidder.MaxMoney;
    }

    public bool CanBid(int currentPrice)
    {
        return CurrentMoney > currentPrice;
    }
}

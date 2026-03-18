using System.Collections.Generic;
using UnityEngine;

public static class AuctionAI
{
    public static BidResultStruct TryGetBid(List<NPCBidderRuntime> bidders, int currentPrice)
    {
        if (bidders == null || bidders.Count == 0)
        {
            return BidResultStruct.Fail();
        }

        var bidder = bidders[Random.Range(0, bidders.Count)];

        if (!bidder.CanBid(currentPrice))
        {
            return BidResultStruct.Fail();
        }

        if (!AuctionUtility.ShouldBid(bidder.Data.Aggressiveness))
        {
            return BidResultStruct.Fail();
        }

        int bidAmount = AuctionUtility.GenerateBidAmount(
            currentPrice,
            bidder.Data.MinBidMultiplier,
            bidder.Data.MaxBidMultiplier
        );

        int newPrice = currentPrice + bidAmount;

        if (newPrice > bidder.CurrentMoney)
        {
            return BidResultStruct.Fail();
        }

        return new BidResultStruct()
        {
            success = true,
            bidder = bidder,
            bidAmount = bidAmount,
            newPrice = newPrice
        };
    }
}

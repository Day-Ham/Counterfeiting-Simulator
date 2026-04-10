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

        if (!AuctionUtility.ShouldBid(bidder.data.aggressiveness))
        {
            return BidResultStruct.Fail();
        }

        int bidAmount = AuctionUtility.GenerateBidAmount(
            currentPrice,
            bidder.data.minBidMultiplier,
            bidder.data.maxBidMultiplier
        );

        int newPrice = currentPrice + bidAmount;

        if (newPrice > bidder.currentMoney)
        {
            return BidResultStruct.Fail();
        }

        return new BidResultStruct()
        {
            Success = true,
            Bidder = bidder,
            BidAmount = bidAmount,
            NewPrice = newPrice
        };
    }
}

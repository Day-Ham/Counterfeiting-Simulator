using UnityEngine;

public static class AuctionUtility
{
    public static int GenerateStartingPrice()
    {
        int startingPrice = Random.Range(100000, 1000000);
        int digits = Mathf.FloorToInt(Mathf.Log10(startingPrice));
        int biddingRange = Mathf.FloorToInt(Mathf.Pow(10, Mathf.Max(1, digits - 1)));

        return biddingRange * Random.Range(1, 75);
    }

    public static int GenerateBidAmount(int currentPrice, int minBid, int maxBid)
    {
        int digits = Mathf.FloorToInt(Mathf.Log10(currentPrice));
        int baseRange = Mathf.FloorToInt(Mathf.Pow(10, Mathf.Max(1, digits - 1)));

        int multiplier = Random.Range(minBid, maxBid + 1);
        return baseRange * multiplier;
    }

    public static int GetSmoothStep(int targetValue)
    {
        int digits = Mathf.FloorToInt(Mathf.Log10(targetValue));

        if (digits <= 4) return Random.Range(100, 1000);
        if (digits == 5) return Random.Range(10000, 50000);
        if (digits == 6) return Random.Range(100000, 500000);

        return Random.Range(1000000, 5000000);
    }

    public static bool ShouldBid(float aggressiveness)
    {
        return Random.value <= aggressiveness;
    }

    public static int DecreaseWantValue(int current)
    {
        current -= Random.Range(5, 15);

        if (Random.Range(1, 25) == 1)
        {
            current = 100;
        }

        return current;
    }
}

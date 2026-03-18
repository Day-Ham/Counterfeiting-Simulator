using UnityEngine;

public static class AuctionUtility
{
    private const float DURATION = 0.06f;
    private const float UPDATE_RATE = 0.04f; //Numbers in FPS update
    
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

    public static int GetSmoothStep(int currentValue, int targetValue)
    {
        int remaining = targetValue - currentValue;
        if (remaining <= 0) return 0;

        // Number of updates during the animation
        int steps = Mathf.CeilToInt(DURATION / UPDATE_RATE);

        // Constant step per update
        int step = Mathf.Max(1, Mathf.CeilToInt(remaining / (float)steps));

        return Mathf.Clamp(step, 1, remaining);
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

using UnityEngine;

public struct ChannelStateStruct
{
    public float CurrentValue; // normalized 0–1
    private int ClosestDifference;

    public ChannelStateStruct(float currentValue)
    {
        CurrentValue = currentValue;
        ClosestDifference = int.MaxValue;
    }

    public void TrySnap(int playerValue, int targetValue, float targetNormalized, int tolerance)
    {
        int diff = Mathf.Abs(playerValue - targetValue);
        if (diff <= tolerance && diff < ClosestDifference)
        {
            ClosestDifference = diff;
            CurrentValue = targetNormalized;
        }
    }
}

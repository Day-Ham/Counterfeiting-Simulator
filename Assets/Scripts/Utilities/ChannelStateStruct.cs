using UnityEngine;

public struct ChannelStateStruct
{
    public float CurrentValue; // normalized 0–1
    private int _closestDifference;

    public ChannelStateStruct(float currentValue)
    {
        CurrentValue = currentValue;
        _closestDifference = int.MaxValue;
    }

    public void TrySnap(int playerValue, int targetValue, float targetNormalized, int tolerance)
    {
        int diff = Mathf.Abs(playerValue - targetValue);
        if (diff > tolerance || diff >= _closestDifference) return;
        
        _closestDifference = diff;
        CurrentValue = targetNormalized;
    }
}

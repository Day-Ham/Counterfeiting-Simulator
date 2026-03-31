using System;
using UnityEngine;

[Serializable]
public struct ComparisonResultStruct
{
    public float similarity;
    public float firstTwoDigits;
    public float lastTwoDigits;

    public ComparisonResultStruct(float similarity, float firstTwoDigits, float lastTwoDigits)
    {
        this.similarity = similarity;
        this.firstTwoDigits = firstTwoDigits;
        this.lastTwoDigits = lastTwoDigits;
    }
    
    public float Percentage => similarity * 100f;
}

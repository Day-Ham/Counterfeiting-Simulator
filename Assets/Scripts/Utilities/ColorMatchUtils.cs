using System.Collections.Generic;
using UnityEngine;

public class ColorMatchUtils
{
    /// <summary>
    /// Returns a color where each RGB channel is snapped to the closest target within tolerance.
    /// Channels are independent.
    /// </summary>
    public static Color SnapPerChannelClosest(Color playerColor, List<Color> targets, int tolerance)
    {
        float red = playerColor.r;
        float green = playerColor.g;
        float blue = playerColor.b;

        int playerR = Mathf.RoundToInt(playerColor.r * 255f);
        int playerG = Mathf.RoundToInt(playerColor.g * 255f);
        int playerB = Mathf.RoundToInt(playerColor.b * 255f);

        int closestRedDifference = tolerance + 1;
        int closestGreenDifference = tolerance + 1;
        int closestBDifference = tolerance + 1;

        foreach (var colorTarget in targets)
        {
            int targetRed = Mathf.RoundToInt(colorTarget.r * 255f);
            int targetGreen = Mathf.RoundToInt(colorTarget.g * 255f);
            int targetBlue = Mathf.RoundToInt(colorTarget.b * 255f);

            red = SnapChannel(playerR, targetRed, colorTarget.r, tolerance, ref closestRedDifference, red);
            green = SnapChannel(playerG, targetGreen, colorTarget.g, tolerance, ref closestGreenDifference, green);
            blue = SnapChannel(playerB, targetBlue, colorTarget.b, tolerance, ref closestBDifference, blue);
        }

        return new Color(red, green, blue);
    }

    /// <summary>
    /// Snaps a single channel to target if it is closer and within tolerance.
    /// </summary>
    private static float SnapChannel(int playerValue, int targetValue, float targetNormalized, int tolerance, ref int closestDifference, float currentChannel)
    {
        int diff = Mathf.Abs(playerValue - targetValue);
        if (diff <= tolerance && diff < closestDifference)
        {
            closestDifference = diff;
            return targetNormalized;
        }
        return currentChannel;
    }
}

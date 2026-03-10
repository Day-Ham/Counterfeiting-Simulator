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
        int playerR = ColorUtils.Red(playerColor);
        int playerG = ColorUtils.Green(playerColor);
        int playerB = ColorUtils.Blue(playerColor);

        var redState = new ChannelStateStruct(playerColor.r);
        var greenState = new ChannelStateStruct(playerColor.g);
        var blueState = new ChannelStateStruct(playerColor.b);

        foreach (var target in targets)
        {
            redState.TrySnap(playerR, ColorUtils.Red(target), target.r, tolerance);
            greenState.TrySnap(playerG, ColorUtils.Green(target), target.g, tolerance);
            blueState.TrySnap(playerB, ColorUtils.Blue(target), target.b, tolerance);
        }

        return new Color(redState.CurrentValue, greenState.CurrentValue, blueState.CurrentValue);
    }
}

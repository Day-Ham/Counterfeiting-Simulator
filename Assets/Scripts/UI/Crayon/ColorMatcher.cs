using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Color/Color Snap Config")]
public class ColorMatcher : ScriptableObject
{
    [Range(0,50)]
    public int tolerance = 10;

    public Color SnapPerChannelClosest(Color playerColor, List<Color> targets)
    {
        return ColorMatchUtils.SnapPerChannelClosest(playerColor, targets, tolerance);
    }
}

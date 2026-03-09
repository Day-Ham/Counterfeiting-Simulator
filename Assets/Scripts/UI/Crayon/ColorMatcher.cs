using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Color/Color Snap Config")]
public class ColorMatcher : ScriptableObject
{
    [Range(0,50)]
    public int tolerance = 10;

    public Color SnapPerChannel(Color playerColor, List<Color> targets)
    {
        float r = playerColor.r;
        float g = playerColor.g;
        float b = playerColor.b;

        foreach (var target in targets)
        {
            int targetR = Mathf.RoundToInt(target.r * 255);
            int targetG = Mathf.RoundToInt(target.g * 255);
            int targetB = Mathf.RoundToInt(target.b * 255);

            int playerR = Mathf.RoundToInt(r * 255);
            int playerG = Mathf.RoundToInt(g * 255);
            int playerB = Mathf.RoundToInt(b * 255);

            if (Mathf.Abs(playerR - targetR) <= tolerance)
                r = target.r; // snap just R

            if (Mathf.Abs(playerG - targetG) <= tolerance)
                g = target.g; // snap just G

            if (Mathf.Abs(playerB - targetB) <= tolerance)
                b = target.b; // snap just B
        }

        return new Color(r, g, b);
    }
}

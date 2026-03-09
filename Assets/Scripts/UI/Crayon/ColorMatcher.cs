using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Color/Color Snap Config")]
public class ColorMatcher : ScriptableObject
{
    [Range(0,50)]
    public int tolerance = 10;

    public Color SnapPerChannelClosest(Color playerColor, List<Color> targets)
    {
        float r = playerColor.r;
        float g = playerColor.g;
        float b = playerColor.b;
        
        int playerR = Mathf.RoundToInt(playerColor.r * 255f);
        int playerG = Mathf.RoundToInt(playerColor.g * 255f);
        int playerB = Mathf.RoundToInt(playerColor.b * 255f);
        
        int closestRDiff = tolerance + 1;
        int closestGDiff = tolerance + 1;
        int closestBDiff = tolerance + 1;

        foreach (var target in targets)
        {
            int targetR = Mathf.RoundToInt(target.r * 255f);
            int targetG = Mathf.RoundToInt(target.g * 255f);
            int targetB = Mathf.RoundToInt(target.b * 255f);

            int diffR = Mathf.Abs(playerR - targetR);
            int diffG = Mathf.Abs(playerG - targetG);
            int diffB = Mathf.Abs(playerB - targetB);
            
            if (diffR <= tolerance && diffR < closestRDiff)
            {
                r = target.r;
                closestRDiff = diffR;
            }
            
            if (diffG <= tolerance && diffG < closestGDiff)
            {
                g = target.g;
                closestGDiff = diffG;
            }
            
            if (diffB <= tolerance && diffB < closestBDiff)
            {
                b = target.b;
                closestBDiff = diffB;
            }
        }

        return new Color(r, g, b);
    }
}

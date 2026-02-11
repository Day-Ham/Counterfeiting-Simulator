using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFormattingUtility
{
    public static string FormatPercentage(float major, float minor, bool showPercent = true)
    {
        string minorText = minor.ToString("00");
        string result = $"{major}<size=70%>.{minorText}</size>";

        if (showPercent)
        {
            result += "<size=60%>%</size>";
        }

        return result;
    }
    
    public static void SetTextList(List<TextMeshProUGUI> list, string text)
    {
        foreach (var textMeshProUGUI in list)
        {
            if (textMeshProUGUI)
            {
                textMeshProUGUI.SetText(text);
            }
        }
    }
    
    public static void SetColorList(List<TextMeshProUGUI> list, Color color)
    {
        foreach (var textMeshProUGUI in list)
        {
            if (textMeshProUGUI)
            {
                textMeshProUGUI.color = color;
            }
        }
    }
}

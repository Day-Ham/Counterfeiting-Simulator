using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFormattingUtility
{
    private const string MajorDigitSize = "100%";
    private const string MinorDigitSize = "50%";
    
    public static string FormatPercentage(float majorDigits, float minorDigits, bool showPercent = true)
    {
        string minorText = minorDigits.ToString("00");
        string formatResult = $"<size={MajorDigitSize}>{majorDigits}</size>" +
                              $"<size={MinorDigitSize}>.{minorText}</size>";

        if (showPercent)
        {
            formatResult += $"<size={MajorDigitSize}>%</size>";
        }

        return formatResult;
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

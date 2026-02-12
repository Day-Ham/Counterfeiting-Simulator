using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFormattingUtility
{
    private const string MAJOR_DIGIT_SIZE = "100%";
    private const string MINOR_DIGIT_SIZE = "50%";
    
    public static string FormatPercentage(float majorDigits, float minorDigits, bool showPercent = true)
    {
        string minorText = minorDigits.ToString("00");
        string formatResult = $"<size={MAJOR_DIGIT_SIZE}>{majorDigits}</size>" +
                              $"<size={MINOR_DIGIT_SIZE}>.{minorText}</size>";

        if (showPercent)
        {
            formatResult += $"<size={MAJOR_DIGIT_SIZE}>%</size>";
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

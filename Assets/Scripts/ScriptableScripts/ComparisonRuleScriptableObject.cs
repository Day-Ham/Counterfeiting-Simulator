using UnityEngine;

[CreateAssetMenu(fileName = "New Comparison Rule", menuName = "Rules/Comparison")]
public class ComparisonRuleScriptableObject : ScriptableObject
{
    [Header("Threshold for Passing (%)")]
    public float PercentRequirement;
    
    private Color PassedColor;
    private Color FailedColor;

    private const string PASSEDCOLOR = "#8FFF86";
    private const string FAILEDCOLOR = "#FF4040";
    
    private void OnEnable()
    {
        ColorUtility.TryParseHtmlString(PASSEDCOLOR, out PassedColor);
        ColorUtility.TryParseHtmlString(FAILEDCOLOR, out FailedColor);
    }
    
    public bool IsPassed(float similarity)
    {
        return similarity * 100f > PercentRequirement;
    }
    
    public Color GetResultColor(float similarity)
    {
        if (IsPassed(similarity))
        {
            ColorUtility.TryParseHtmlString("#8FFF86", out var passColor);
            return passColor;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#FF4040", out var failColor);
            return failColor;
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Comparison Rule", menuName = "Rules/Comparison")]
public class ComparisonRuleScriptableObject : ScriptableObject
{
    [Header("Threshold for Passing (%)")]
    public float percentRequirement;
    
    private Color _passedColor;
    private Color _failedColor;

    private const string PassedColor = "#8FFF86";
    private const string FailedColor = "#FF4040";
    
    private void OnEnable()
    {
        ColorUtility.TryParseHtmlString(PassedColor, out _passedColor);
        ColorUtility.TryParseHtmlString(FailedColor, out _failedColor);
    }
    
    public bool IsPassed(float similarity)
    {
        return similarity * 100f > percentRequirement;
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

using UnityEngine;

[CreateAssetMenu(fileName = "New Comparison Rule", menuName = "Rules/Comparison")]
public class ComparisonRuleScriptableObject : ScriptableObject
{
    [Header("Threshold for Passing (%)")]
    public float PercentRequirement;
    
    private Color PassedColor;
    private Color FailedColor;

    private const string PASSEDCOLOR = "#8FFF86";
    private const string FAILEDCOLOR = "#FF7575";
    
    private void Awake()
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
            return PassedColor;
        }
        else
        {
            return FailedColor;
        }
    }
}

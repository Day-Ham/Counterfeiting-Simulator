using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PercentageTextAssignments : MonoBehaviour
{
    public List<TextMeshProUGUI> PercentageText;
    public List<TextMeshProUGUI> PercentageTextShadow;
    
    public TMPListValue PercentageTextScriptableObject;
    public TMPListValue PercentageTextShadowScriptableObject;

    private void Start()
    {
        ClearTexts(PercentageText, PercentageTextShadow);
        
        PercentageTextScriptableObject.Value = PercentageText;
        PercentageTextShadowScriptableObject.Value = PercentageTextShadow;
    }
    
    private void ClearTexts(params List<TextMeshProUGUI>[] percentageUIList)
    {
        foreach (var percentageUIListTMP in percentageUIList)
        {
            percentageUIListTMP.ForEach(textMeshProUGUI => { if (textMeshProUGUI != null) textMeshProUGUI.text = ""; });
        }
    }
}

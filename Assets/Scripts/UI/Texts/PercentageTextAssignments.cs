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
        PercentageTextScriptableObject.Value = PercentageText;
        PercentageTextShadowScriptableObject.Value = PercentageTextShadow;
    }
}

using System.Collections;
using System.Collections.Generic;
using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ComparingMechanic : MonoBehaviour
{
    public GameObject TargetLocation;

    public SceneChanger sceneChanger;
    public GameObject ObjectToMove;
    public GameObject ObjectToMove2;

    public Ease EaseTween;
    public Ease PromptTween;

    public GameObject PercentageParent;

    [Header("Main Text ScriptableObject")]
    public TMPListValue PercentageTextScriptableObject;

    [Header("Shadow Text ScriptableObject")]
    public TMPListValue PercentageTextShadowScriptableObject;
    
    [Header("Colors")]
    public Color PassedColor;
    public Color FailedColor;
    public Color ShadowColor;
    
    public GameObject ResetButton;
    public GameManager GameManager;
    
    private float majorPercentageNumber;
    private float minorPercentageNumber;
    
    [SerializeField] private int duration = 100;
    [SerializeField] private float threshold = 80;
    
    public Color InspectionColor;
    private bool OneShot = true;
    
    private void Start()
    {
        PercentageParent.transform.localScale = Vector3.zero;
        ObjectToMove2.GetComponent<RawImage>().color = Color.white;
        
        SetColorList(PercentageTextShadowScriptableObject.Value, ShadowColor);
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !OneShot) return;
        
        GameManager.FinishGame();
        
        ObjectToMove.transform
            .DOMove(TargetLocation.transform.position, 1f, false)
            .SetEase(EaseTween)
            .OnComplete(() => { StartCoroutine(ShowResult()); });
        
        ObjectToMove2.GetComponent<RawImage>().color= InspectionColor;
        
        ObjectToMove2.transform
            .DOMove(TargetLocation.transform.position, 1f, false)
            .SetEase(EaseTween);
        
        OneShot =false;
    }
    
    private IEnumerator ShowResult()
    {
        PercentageParent.transform
            .DOScale(Vector3.one, 1f)
            .SetEase(PromptTween);
        
        for (int tick = 0; tick <= duration; tick++)
        { 
            yield return new WaitForSeconds(0.02f);
            
            majorPercentageNumber = Random.Range(1, 99);
            minorPercentageNumber = Random.Range(1, 99);
            
            SetPercentageText(majorPercentageNumber, minorPercentageNumber);
            
            if (tick != duration) continue;
            
            SetPercentageText(GameManager.FirstTwoDigits, GameManager.LastTwoDigits);
        }
        
        Debug.Log(GameManager.allSimilarity * 100);
        Debug.Log(threshold);
        Debug.Log(GameManager.allSimilarity * 100 > threshold);
        
        CheckingSimilar();
        
        yield return new WaitForSeconds(1f);
        
        Similar();
    }
    
    private void SetPercentageText(float major, float minor)
    {
        string formatted = FormatPercentage(major, minor);

        SetTextList(PercentageTextScriptableObject.Value, formatted);
        SetTextList(PercentageTextShadowScriptableObject.Value, formatted);
    }
    
    private void SetTextList(List<TextMeshProUGUI> list, string numberText)
    {
        foreach (var text in list)
        {
            if (text)
            {
                text.SetText(numberText);
            }
        }
    }

    private void CheckingSimilar()
    {
        if (GameManager.allSimilarity * 100 < threshold)
        {
            SetColorList(PercentageTextScriptableObject.Value, PassedColor);
        }
        else
        {
            SetColorList(PercentageTextScriptableObject.Value, FailedColor);
        }
    }
    
    private void SetColorList(List<TextMeshProUGUI> list, Color colorPassFail)
    {
        foreach (var text in list)
        {
            if (text)
            {
                text.color = colorPassFail;
            }
        }
    }
    
    private string FormatPercentage(float major, float minor, bool showPercent = true)
    {
        string minorText = minor.ToString("00");

        string result = $"{major}<size=70%>.{minorText}</size>";

        if (showPercent)
        {
            result += "<size=60%>%</size>";
        }

        return result;
    }

    private void Similar()
    {
        if (GameManager.allSimilarity * 100 > threshold)
        {
            sceneChanger.ShowNextButton();
        }
        else
        {
            StartCoroutine(IndicateReset());
        }
    }

    private IEnumerator IndicateReset()
    {
        ResetButton.transform.DOScale(Vector3.one *1.25f, 1f).SetEase(PromptTween);
        
        yield return new WaitForSeconds(.75f);
        
        ResetButton.transform.DOScale(Vector3.one, 1f).SetEase(PromptTween);
        
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(IndicateReset());
    }
}

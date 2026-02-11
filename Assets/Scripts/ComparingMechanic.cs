using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ComparingMechanic : MonoBehaviour
{
    public GameObject TargetLocation;

    public SceneChanger sceneChanger;
    public GameObject ObjectToMove;
    public GameObject ObjectToMove2;

    public Ease EaseTween;
    public Ease PromptTween;
    
    [Header("Comparison ScriptableObject")]
    public GameObjectValue PercentageParent;
    public ResizeTweenScriptableObject ResizeTween;
    
    [Header("Comparison ScriptableObject")]
    public ComparisonRuleScriptableObject ComparisonRule;

    [Header("Main Text ScriptableObject")]
    public TMPListValue PercentageTextScriptableObject;

    [Header("Shadow Text ScriptableObject")]
    public TMPListValue PercentageTextShadowScriptableObject;
    
    public GameObject ResetButton;
    public GameManager GameManager;
    
    private float majorPercentageNumber;
    private float minorPercentageNumber;
    
    [SerializeField] private int duration = 100;
    
    public Color InspectionColor;
    private bool OneShot = true;
    
    private void Start()
    {
        ResizeTween.Collapse(PercentageParent.Value);
        ObjectToMove2.GetComponent<RawImage>().color = Color.white;
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
        ResizeTween.Expand(PercentageParent.Value);
        
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
        Debug.Log(ComparisonRule.PercentRequirement);
        Debug.Log(GameManager.allSimilarity * 100 > ComparisonRule.PercentRequirement);
        
        CheckingSimilar();
        
        yield return new WaitForSeconds(1f);
        
        Similar();
    }
    
    private void SetPercentageText(float major, float minor)
    {
        string formatted = TextFormattingUtility.FormatPercentage(major, minor);

        TextFormattingUtility.SetTextList(PercentageTextScriptableObject.Value, formatted);
        TextFormattingUtility.SetTextList(PercentageTextShadowScriptableObject.Value, formatted);
    }

    private void CheckingSimilar()
    {
        Color resultColor = ComparisonRule.GetResultColor(GameManager.allSimilarity);
        TextFormattingUtility.SetColorList(PercentageTextScriptableObject.Value, resultColor);
    }

    private void Similar()
    {
        if (ComparisonRule.IsPassed(GameManager.allSimilarity))
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

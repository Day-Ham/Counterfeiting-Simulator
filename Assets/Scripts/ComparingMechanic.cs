using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ComparingMechanic : MonoBehaviour
{
    public SceneChanger sceneChanger;
    
    [Header("GameObject to Move")]
    public GameObjectValue TargetImage;
    public GameObjectValue FrontSilhouette;
    
    [Header("Target Location")]
    public GameObject TargetLocation;
    
    public Ease EaseTween;
    
    [Header("Percentage ScriptableObject")]
    public GameObjectValue PercentageParent;
    public ResizeTweenScriptableObject PercentageResizeTween;
    
    [Header("ResetButton ScriptableObject")]
    public GameObjectValue ResetButtonUI;
    public ResizeTweenScriptableObject ResetButtonUIResizeTween;
    
    [Header("Comparison ScriptableObject")]
    public ComparisonRuleScriptableObject ComparisonRule;

    [Header("Main Text ScriptableObject")]
    public TMPListValue PercentageTextScriptableObject;

    [Header("Shadow Text ScriptableObject")]
    public TMPListValue PercentageTextShadowScriptableObject;
    
    [Header("GameManager Events")]
    public GameManagerEvents FinishGameRequestEvent;
    public ComparisonResultEvent ComparisonResultEvent;
    
    private float majorPercentageNumber;
    private float minorPercentageNumber;
    
    [SerializeField] private int duration = 100;
    
    public Color InspectionColor;
    private bool OneShot = true;
    
    private float gameManagerCachedSimilarity;
    private float gameManagerCachedFirstTwo;
    private float gameManagerCachedLastTwo;
    
    private void OnEnable()
    {
        ComparisonResultEvent.OnRaised += OnComparisonFinished;
    }

    private void OnDisable()
    {
        ComparisonResultEvent.OnRaised -= OnComparisonFinished;
    }

    private void OnComparisonFinished(float similarity, float firstTwo, float lastTwo)
    {
        gameManagerCachedSimilarity = similarity;
        gameManagerCachedFirstTwo = firstTwo;
        gameManagerCachedLastTwo = lastTwo;
    }
    
    private void Start()
    {
        PercentageResizeTween.Collapse(PercentageParent.Value);
        FrontSilhouette.Value.GetComponent<RawImage>().color = Color.white;
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !OneShot) return;
        
        FinishGameRequestEvent.Raise();//Game Finish
        
        TargetImage.Value.transform
            .DOMove(TargetLocation.transform.position, 1f, false)
            .SetEase(EaseTween)
            .OnComplete(() => { StartCoroutine(ShowResult()); });
        
        FrontSilhouette.Value.GetComponent<RawImage>().color= InspectionColor;
        
        FrontSilhouette.Value.transform
            .DOMove(TargetLocation.transform.position, 1f, false)
            .SetEase(EaseTween);
        
        OneShot =false;
    }
    
    private IEnumerator ShowResult()
    {
        PercentageResizeTween.Expand(PercentageParent.Value);
        
        for (int tick = 0; tick <= duration; tick++)
        { 
            yield return new WaitForSeconds(0.02f);
            
            majorPercentageNumber = Random.Range(1, 99);
            minorPercentageNumber = Random.Range(1, 99);
            
            SetPercentageText(majorPercentageNumber, minorPercentageNumber);
            
            if (tick != duration) continue;
            
            SetPercentageText(gameManagerCachedFirstTwo, gameManagerCachedLastTwo);
        }
        
        Debug.Log(gameManagerCachedSimilarity * 100);
        Debug.Log(ComparisonRule.PercentRequirement);
        Debug.Log(gameManagerCachedSimilarity * 100 > ComparisonRule.PercentRequirement);
        
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
        Color resultColor = ComparisonRule.GetResultColor(gameManagerCachedSimilarity);
        TextFormattingUtility.SetColorList(PercentageTextScriptableObject.Value, resultColor);
    }

    private void Similar()
    {
        if (ComparisonRule.IsPassed(gameManagerCachedSimilarity))
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
        ResetButtonUIResizeTween.Expand(ResetButtonUI.Value);
        
        yield return new WaitForSeconds(.75f);
        
        ResetButtonUIResizeTween.Collapse(ResetButtonUI.Value);
        
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(IndicateReset());
    }
}

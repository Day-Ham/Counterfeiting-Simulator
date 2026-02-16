using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ComparingMechanic : MonoBehaviour
{
    public VoidEvent SceneChangerEvent;
    
    [Header("GameObject to Move")]
    public GameObjectValue TargetImage;
    public GameObjectValue FrontSilhouette;
    
    [Header("Target Location")]
    public GameObjectValue TargetLocation;
    
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
    private bool _isOneShot = true;
    
    private float _gameManagerCachedSimilarity;
    private float _gameManagerCachedFirstTwo;
    private float _gameManagerCachedLastTwo;
    
    private RectTransform _targetImageRect;
    private RectTransform _frontRect;
    private RectTransform _targetLocationRect;
    private RawImage _frontRawImage;
    
    private void Awake()
    {
        _targetImageRect = TargetImage.Value.GetComponent<RectTransform>();
        _frontRect = FrontSilhouette.Value.GetComponent<RectTransform>();
        _targetLocationRect = TargetLocation.Value.GetComponent<RectTransform>();
        _frontRawImage = FrontSilhouette.Value.GetComponent<RawImage>();
    }
    
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
        _gameManagerCachedSimilarity = similarity;
        _gameManagerCachedFirstTwo = firstTwo;
        _gameManagerCachedLastTwo = lastTwo;
    }
    
    private void Start()
    {
        PercentageResizeTween.Collapse(PercentageParent.Value);
        FrontSilhouette.Value.GetComponent<RawImage>().color = Color.white;
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !_isOneShot) return;
        
        FinishGameRequestEvent.Raise();
        
        Vector3 targetPos = _targetLocationRect.position;

        _targetImageRect
            .DOMove(targetPos, 1f)
            .SetEase(EaseTween)
            .OnComplete(() => StartCoroutine(ShowResult()));

        _frontRect
            .DOMove(targetPos, 1f)
            .SetEase(EaseTween);

        _frontRawImage.color = InspectionColor;
        
        _isOneShot =false;
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
            
            SetPercentageText(_gameManagerCachedFirstTwo, _gameManagerCachedLastTwo);
        }
        
        Debug.Log(_gameManagerCachedSimilarity * 100);
        Debug.Log(ComparisonRule.PercentRequirement);
        Debug.Log(_gameManagerCachedSimilarity * 100 > ComparisonRule.PercentRequirement);
        
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
        Color resultColor = ComparisonRule.GetResultColor(_gameManagerCachedSimilarity);
        TextFormattingUtility.SetColorList(PercentageTextScriptableObject.Value, resultColor);
    }

    private void Similar()
    {
        if (ComparisonRule.IsPassed(_gameManagerCachedSimilarity))
        {
            //sceneChanger.ShowNextButton();
            
            SceneChangerEvent.Raise();
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

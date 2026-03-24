using System.Collections;
using DaeHanKim.ThisIsTotallyADollar.Core;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ComparingMechanic : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent sceneChangerEvent;
    [SerializeField] private VoidEvent drawingBoardControllerEvent;
    [SerializeField] private VoidEvent startCompareEvent;
    [SerializeField] private VoidEvent finishGameRequestEvent;
    [SerializeField] private VoidEvent spacePressedEvent;
    [SerializeField] private ComparisonResultEvent comparisonResultEvent;
    
    [Header("GameObject to Move")]
    [SerializeField] private GameObjectValue targetImage; 
    [SerializeField] private GameObjectValue frontSilhouette;
    
    [Header("Target Location")]
    [SerializeField] private GameObjectValue targetLocation;
    [SerializeField] private Ease easeTween;
    
    [Header("Percentage ScriptableObject")]
    [SerializeField] private RectTransformValue percentageParent;
    [SerializeField] private ResizeTweenScriptableObject percentageResizeTween;
    
    [Header("ResetButton ScriptableObject")]
    [SerializeField] private RectTransformValue resetButtonUI;
    [SerializeField] private ResizeTweenScriptableObject resetButtonUIResizeTween;
    
    [Header("Comparison ScriptableObject")]
    [SerializeField] private ComparisonRuleScriptableObject comparisonRule;

    [Header("Main Text ScriptableObject")]
    [SerializeField] private TMPListValue percentageTextScriptableObject;

    [Header("Shadow Text ScriptableObject")]
    [SerializeField] private TMPListValue percentageTextShadowScriptableObject;
    
    [SerializeField] private int duration = 100;
    [SerializeField] private Color inspectionColor;
    
    private float _majorPercentageNumber;
    private float _minorPercentageNumber;
    
    private bool _isOneShot = true;
    
    private float _gameManagerCachedSimilarity;
    private float _gameManagerCachedFirstTwo;
    private float _gameManagerCachedLastTwo;
    
    private RectTransform _targetImageRect;
    private RectTransform _frontRect;
    private RectTransform _targetLocationRect;
    private RawImage _frontRawImage;
    
    private void OnEnable()
    {
        comparisonResultEvent.OnRaised += OnComparisonFinished;
        startCompareEvent.Register(StartCompare);
        spacePressedEvent.Register(OnSpacePressed);
    }

    private void OnDisable()
    {
        comparisonResultEvent.OnRaised -= OnComparisonFinished;
        startCompareEvent.Unregister(StartCompare);
        spacePressedEvent.Unregister(OnSpacePressed);
        
    }

    private void OnComparisonFinished(float similarity, float firstTwo, float lastTwo)
    {
        _gameManagerCachedSimilarity = similarity;
        _gameManagerCachedFirstTwo = firstTwo;
        _gameManagerCachedLastTwo = lastTwo;
    }
    
    private void Start()
    {
        _targetImageRect = targetImage.Value.GetComponent<RectTransform>();
        _frontRect = frontSilhouette.Value.GetComponent<RectTransform>();
        _targetLocationRect = targetLocation.Value.GetComponent<RectTransform>();
        _frontRawImage = frontSilhouette.Value.GetComponent<RawImage>();
        
        percentageResizeTween.Collapse(percentageParent.Value);
        frontSilhouette.Value.GetComponent<RawImage>().color = Color.white;
    }
    
    private void OnSpacePressed()
    {
        startCompareEvent.Raise();
    }
    
    private void StartCompare()
    {
        if (!_isOneShot) return;

        drawingBoardControllerEvent.Raise();
        finishGameRequestEvent.Raise();

        Vector3 targetPos = _targetLocationRect.position;

        _targetImageRect
            .DOMove(targetPos, 1f)
            .SetEase(easeTween)
            .OnComplete(() => StartCoroutine(ShowResult()));

        _frontRect
            .DOMove(targetPos, 1f)
            .SetEase(easeTween);

        _frontRawImage.color = inspectionColor;

        _isOneShot = false;
    }
    
    private IEnumerator ShowResult()
    {
        percentageResizeTween.Expand(percentageParent.Value);
        
        for (int tick = 0; tick <= duration; tick++)
        { 
            yield return new WaitForSeconds(0.02f);
            
            _majorPercentageNumber = Random.Range(1, 99);
            _minorPercentageNumber = Random.Range(1, 99);
            
            SetPercentageText(_majorPercentageNumber, _minorPercentageNumber);
            
            if (tick != duration) continue;
            
            SetPercentageText(_gameManagerCachedFirstTwo, _gameManagerCachedLastTwo);
        }
        
        Debug.Log(_gameManagerCachedSimilarity * 100);
        Debug.Log(comparisonRule.PercentRequirement);
        Debug.Log(_gameManagerCachedSimilarity * 100 > comparisonRule.PercentRequirement);
        
        CheckingSimilar();
        
        yield return new WaitForSeconds(1f);
        
        Similar();
    }
    
    private void SetPercentageText(float major, float minor)
    {
        string formatted = TextFormattingUtility.FormatPercentage(major, minor);

        TextFormattingUtility.SetTextList(percentageTextScriptableObject.Value, formatted);
        TextFormattingUtility.SetTextList(percentageTextShadowScriptableObject.Value, formatted);
    }

    private void CheckingSimilar()
    {
        Color resultColor = comparisonRule.GetResultColor(_gameManagerCachedSimilarity);
        TextFormattingUtility.SetColorList(percentageTextScriptableObject.Value, resultColor);
    }

    private void Similar()
    {
        if (comparisonRule.IsPassed(_gameManagerCachedSimilarity))
        {
            sceneChangerEvent.Raise();
        }
        else
        {
            StartCoroutine(IndicateReset());
        }
    }

    private IEnumerator IndicateReset()
    { 
        resetButtonUIResizeTween.Expand(resetButtonUI.Value);
        
        yield return new WaitForSeconds(.75f);
        
        resetButtonUIResizeTween.Collapse(resetButtonUI.Value);
        
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(IndicateReset());
    }
}

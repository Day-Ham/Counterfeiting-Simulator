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
    [SerializeField] private VoidEvent finishGameRequestEvent;
    [SerializeField] private VoidEvent spacePressedEvent;
    [SerializeField] private VoidEvent onShowPercentageUI;
    [SerializeField] private VoidEvent indicateResetButtonEvent;
    [SerializeField] private ComparisonResultEvent comparisonResultEvent;
    
    [Header("GameObject to Move")]
    [SerializeField] private GameObjectValue targetImage; 
    [SerializeField] private GameObjectValue frontSilhouette;
    
    [Header("Target Location")]
    [SerializeField] private GameObjectValue targetLocation;
    [SerializeField] private Ease easeTween;
    
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
    
    private ComparisonResultStruct _cachedResult;
    
    private RectTransform _targetImageRect;
    private RectTransform _frontRect;
    private RectTransform _targetLocationRect;
    private RawImage _frontRawImage;
    
    private void OnEnable()
    {
        comparisonResultEvent.Register(OnComparisonFinished);
        spacePressedEvent.Register(OnSpacePressed);
    }

    private void OnDisable()
    {
        comparisonResultEvent.Unregister(OnComparisonFinished);
        spacePressedEvent.Unregister(OnSpacePressed);
    }

    private void OnComparisonFinished(ComparisonResultStruct result)
    {
        _cachedResult = result;
    }
    
    private void Start()
    {
        _targetImageRect = targetImage.Value.GetComponent<RectTransform>();
        _frontRect = frontSilhouette.Value.GetComponent<RectTransform>();
        _targetLocationRect = targetLocation.Value.GetComponent<RectTransform>();
        _frontRawImage = frontSilhouette.Value.GetComponent<RawImage>();
        
        frontSilhouette.Value.GetComponent<RawImage>().color = Color.white;
    }
    
    private void OnSpacePressed()
    {
        StartCompare();
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
    
    //Animation Result
    private IEnumerator ShowResult()
    {
        onShowPercentageUI.Raise();
        
        for (int tick = 0; tick <= duration; tick++)
        { 
            yield return new WaitForSeconds(0.02f);
            
            _majorPercentageNumber = Random.Range(1, 99);
            _minorPercentageNumber = Random.Range(1, 99);
            
            SetPercentageText(_majorPercentageNumber, _minorPercentageNumber);
            
            if (tick != duration) continue;
            
            SetPercentageText(_cachedResult.firstTwoDigits, _cachedResult.lastTwoDigits);
        }
        
        Debug.Log(_cachedResult.Percentage);
        Debug.Log(comparisonRule.percentRequirement);
        Debug.Log(_cachedResult.Percentage > comparisonRule.percentRequirement);
        
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
        Color resultColor = comparisonRule.GetResultColor(_cachedResult.similarity);
        TextFormattingUtility.SetColorList(percentageTextScriptableObject.Value, resultColor);
    }

    private void Similar()
    {
        if (comparisonRule.IsPassed(_cachedResult.similarity))
        {
            sceneChangerEvent.Raise();
        }
        else
        {
            indicateResetButtonEvent.Raise();
        }
    }
}

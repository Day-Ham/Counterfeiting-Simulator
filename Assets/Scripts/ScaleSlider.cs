using DaeHanKim.ThisIsTotallyADollar.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ScaleSlider : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private FloatEvent brushSizeEvent;
    
    [Space]
    [SerializeField] private Slider brushScaleSlider;
    [SerializeField] private GameObjectValue cursor;

    [SerializeField] private RectTransform sliderRect;
    [SerializeField] private Canvas parentCanvas;
    
    [SerializeField] private float referenceNumber = 64;
    [SerializeField] private float scrollSensitivity;
    
    private void OnEnable()
    {
        GameState.OnGamePaused += HandleGamePaused;
        GameState.OnGameResumed += HandleGameResumed;
    }

    private void OnDisable()
    {
        GameState.OnGamePaused -= HandleGamePaused;
        GameState.OnGameResumed -= HandleGameResumed;
    }
    
    private void Awake()
    {
        if (brushScaleSlider != null)
        {
            brushScaleSlider.onValueChanged.AddListener(SetSize);
        }
    }
    
    private void Update()
    {
        if (GameState.IsGameFinished || GameState.IsGamePaused) return;

        // Only scroll when mouse is over the slider
        if (!RectTransformUtility.RectangleContainsScreenPoint(sliderRect, Input.mousePosition, parentCanvas.worldCamera)) return;
        
        float scroll = InputUtility.MouseWheelDelta;
        
        if (!(Mathf.Abs(scroll) > 0.01f)) return;
        
        float newValue = brushScaleSlider.value + scroll * scrollSensitivity;
        brushScaleSlider.value = Mathf.Clamp(newValue, brushScaleSlider.minValue, brushScaleSlider.maxValue);
    }
    
    
    private void SetSize(float brushScaleSize)
    {
        if (GameState.IsGameFinished || GameState.IsGamePaused) return;

        if (cursor.Value != null)
        {
            cursor.Value.transform.localScale = Vector3.one * brushScaleSize;
        }

        brushSizeEvent.Raise(brushScaleSize * referenceNumber);
    }
    
    private void HandleGamePaused()
    {
        brushScaleSlider.interactable = false;
    }

    private void HandleGameResumed()
    {
        brushScaleSlider.interactable = true;
    }
}

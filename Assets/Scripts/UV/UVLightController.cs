using UnityEngine;
using UnityEngine.UI;

public class UVLightController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private BoolEvent uvToggleEvent;

    [Header("References")]
    [SerializeField] private RawImage uvRawImage;
    [SerializeField] private RectTransform maskPanel;
    [SerializeField] private RectTransform targetRectTransform;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private MainGameConfigRuntimeAsset runtimeAsset;

    private bool _isActive;

    private void OnEnable()
    {
        uvToggleEvent.Register(SetActive);
        GameState.OnGamePaused += HandlePause;
        GameState.OnGameResumed += HandleResume;
    }

    private void OnDisable()
    {
        uvToggleEvent.Unregister(SetActive);
        GameState.OnGamePaused -= HandlePause;
        GameState.OnGameResumed -= HandleResume;
    }

    private void Start()
    {
        if (runtimeAsset?.Value?.UVTexture?.Value != null)
            uvRawImage.texture = runtimeAsset.Value.UVTexture.Value;

        maskPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isActive || GameState.IsGameFinished || GameState.IsGamePaused) return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, Input.mousePosition, parentCanvas.worldCamera, out Vector2 localPoint)) return;

        maskPanel.anchoredPosition = localPoint;
        uvRawImage.rectTransform.anchoredPosition = targetRectTransform.anchoredPosition - localPoint;
    }

    private void SetActive(bool value)
    {
        Debug.Log("UV Light toggled: " + value);
        _isActive = value;
        maskPanel.gameObject.SetActive(value);
    }

    private void HandlePause() => maskPanel.gameObject.SetActive(false);
    private void HandleResume() => maskPanel.gameObject.SetActive(_isActive);
}
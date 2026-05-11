using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Events")]
    [SerializeField] private AudioClipEvent sfxEvent;

    [Header("SFX")]
    [SerializeField] private AudioClipValue grabClip;
    [SerializeField] private AudioClipValue releaseClip;
    [SerializeField] private AudioClipValue tickClip;

    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Settings")]
    [SerializeField] private float tickInterval = 0.05f;

    private float _lastTickTime;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        if (Time.unscaledTime - _lastTickTime < tickInterval) return;
        _lastTickTime = Time.unscaledTime;
        sfxEvent.Raise(tickClip.Value);
    }

    public void OnPointerDown(PointerEventData eventData) => sfxEvent.Raise(grabClip.Value);
    public void OnPointerUp(PointerEventData eventData) => sfxEvent.Raise(releaseClip.Value);
}
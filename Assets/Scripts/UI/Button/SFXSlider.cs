using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioClipEvent sfxEvent;
    [SerializeField] private AudioClip grabClip;
    [SerializeField] private AudioClip releaseClip;
    [SerializeField] private AudioClip tickClip;
    [SerializeField] private Slider slider;
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
        sfxEvent.Raise(tickClip);
    }

    public void OnPointerDown(PointerEventData eventData) => sfxEvent.Raise(grabClip);
    public void OnPointerUp(PointerEventData eventData) => sfxEvent.Raise(releaseClip);
}
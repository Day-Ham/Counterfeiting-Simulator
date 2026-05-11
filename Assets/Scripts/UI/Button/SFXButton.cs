using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SFXButton : MonoBehaviour, IPointerEnterHandler
{
    [Header("Events")]
    [Tooltip("Assign the event to raise when this button is clicked.")]
    [SerializeField] private AudioClipEvent audioClipEvent;
    [Header("SFX")]
    [SerializeField] private AudioClipValue menuhoverSFX;
    [SerializeField] private AudioClipValue menuclickSFX;

    [Header("Button")]
    [SerializeField] private Button button;

    private void Awake() => button.onClick.AddListener(() => audioClipEvent.Raise(menuclickSFX.Value));
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        audioClipEvent.Raise(menuhoverSFX.Value);
    }
}

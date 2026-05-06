using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private PersistentFloatValue volume;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.SetValueWithoutNotify(volume.Value);
        slider.onValueChanged.AddListener(value => volume.Value = value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        volume.Save();
    }

}

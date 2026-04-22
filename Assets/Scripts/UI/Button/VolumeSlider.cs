using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private PersistentFloatValue volume;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.SetValueWithoutNotify(volume.Value);
        slider.onValueChanged.AddListener(value => volume.Value = value);
    }
}
